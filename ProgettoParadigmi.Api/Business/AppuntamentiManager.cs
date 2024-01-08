using System.Drawing;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using ProgettoParadigmi.Api.Utils;
using ProgettoParadigmi.EmailSender;
using ProgettoParadigmi.Models.Context;
using ProgettoParadigmi.Models.Dto;
using ProgettoParadigmi.Models.Entities;
using ProgettoParadigmi.Models.GenericRepository;

namespace ProgettoParadigmi.Api.Business;

public class AppuntamentiManager(AppuntamentiDbContext ctx, EmailService mailService)
{
    private readonly IGenericRepository<Appuntamento> _appuntamenti = new GenericRepository<Appuntamento>(ctx);
    private readonly IGenericRepository<Partecipante> _partecipanti = new GenericRepository<Partecipante>(ctx);
    private readonly IGenericRepository<Utente> _utente = new GenericRepository<Utente>(ctx);


    public Response<List<AppuntamentoDto>> GetAppuntamentiByUserId(Guid userId, int mese, int anno)
    {
        if (userId == Guid.Empty)
            return ResponseFactory.CreateResponseFromResult<List<AppuntamentoDto>>(null, false,
                "L'id non può essere vuoto");
        if (mese == 0) mese = DateTime.Today.Month;
        if (anno == 0) anno = DateTime.Today.Year;
        var appuntamenti = _appuntamenti
            .Query(x => x.Organizzatore.Id == userId
                        && x.DataInizio.Year == anno && x.DataInizio.Month == mese && !x.IsDeleted)
            .Select(x => new AppuntamentoDto
            {
                DataInizio = x.DataInizio,
                Descrizione = x.Descrizione,
                OrganizzatoreId = x.Organizzatore.Id,
                DataFine = x.DataFine,
                Titolo = x.Titolo,
                Partecipanti = x.Partecipanti
                    .Select(y => new UtenteDto(y.Utente.Id, y.Utente.Nome, y.Utente.Cognome, y.Utente.Email,
                        y.Utente.TipoUtente))
                    .ToList(),
                Categoria = new CategoriaDto(x.Categoria.Descrizione, x.Categoria.Id, x.Categoria.Color)
            })
            .ToList();
        // mi prendo anche gli eventi in cui l'utente è partecipante
        try
        {
            var partecipanti = _partecipanti
                .Query(x => x.Evento.DataInizio.Year == anno && x.Evento.DataInizio.Month == mese
                                                             && x.StatoInvito == StatoInvito.Accettato && !x.IsDeleted)
                .Select(x => x.Evento)
                .ToList();
            if (partecipanti.Exists(x => x.Partecipanti.Exists(y => y.UtenteId == userId)))
            {
                var invitati = partecipanti
                    .Select(x => new AppuntamentoDto
                    {
                        DataInizio = x.DataInizio,
                        Descrizione = x.Descrizione,
                        OrganizzatoreId = x.Organizzatore.Id,
                        DataFine = x.DataFine,
                        Titolo = x.Titolo,
                        Partecipanti = x.Partecipanti
                            .Select(y => new UtenteDto(y.Utente.Id, y.Utente.Nome, y.Utente.Cognome, y.Utente.Email,
                                y.Utente.TipoUtente))
                            .ToList(),
                        Categoria = new CategoriaDto(x.Categoria.Descrizione, x.Categoria.Id, x.Categoria.Color)
                    })
                    .ToList();
                if (invitati.Count > 0)
                {
                    appuntamenti.AddRange(invitati);
                }
            }
        }
        catch (Exception e)
        {
        }

        return ResponseFactory.CreateResponseFromResult(appuntamenti);
    }

    public Response<List<AppuntamentoDto>> GetAppuntamentiByCategoriaId(Guid categoriaId, int mese, int anno)
    {
        if (categoriaId == Guid.Empty)
            return ResponseFactory.CreateResponseFromResult<List<AppuntamentoDto>>(null, false,
                "L'id non può essere vuoto");
        if (mese == 0) mese = DateTime.Today.Month;
        if (anno == 0) anno = DateTime.Today.Year;
        var appuntamenti = _appuntamenti
            .Query(x => x.Categoria.Id == categoriaId
                        && x.DataInizio.Year == anno && x.DataInizio.Month == mese && !x.IsDeleted)
            .Select(x => new AppuntamentoDto
            {
                DataInizio = x.DataInizio,
                Descrizione = x.Descrizione,
                OrganizzatoreId = x.Organizzatore.Id,
                DataFine = x.DataFine,
                Titolo = x.Titolo,
                Partecipanti = x.Partecipanti
                    .Select(y => new UtenteDto(y.Utente.Id, y.Utente.Nome, y.Utente.Cognome, y.Utente.Email,
                        y.Utente.TipoUtente))
                    .ToList(),
                Categoria = new CategoriaDto(x.Categoria.Descrizione, x.Categoria.Id, x.Categoria.Color)
            })
            .ToList();
        return ResponseFactory.CreateResponseFromResult(appuntamenti);
    }

    public async Task<Response<bool>> InsertAppuntamento(AppuntamentoDto appuntamento)
    {
        ArgumentNullException.ThrowIfNull(appuntamento);
        if (appuntamento.OrganizzatoreId == Guid.Empty)
        {
            return ResponseFactory.CreateResponseFromResult(false, false,
                "L'id dell'utente non può essere vuoto");
        }

        if (appuntamento.DataFine.HasValue && appuntamento.DataFine < appuntamento.DataInizio)
        {
            return ResponseFactory.CreateResponseFromResult(false, false,
                "La data di fine non può precedere la data di inizio");
        }

        // controllo se la categoria indicata appartiene all'utente
        // ho dovuto mettere un includes altrimenti non caricava le categorie
        var categorie = _utente
            .GetFirstOrDefault(x => x.Id == appuntamento.OrganizzatoreId && !x.IsDeleted, x => x.Categorie)
            .Categorie ?? [];
        var isCategoriaOfUser = categorie.Any(x => x.Id == appuntamento.Categoria.Id);
        if (!isCategoriaOfUser)
        {
            return ResponseFactory.CreateResponseFromResult(false, false,
                "La categoria che è stata indicata non appartiene all'utente selezionato");
        }

        var app = new Appuntamento
        {
            DataInizio = appuntamento.DataInizio,
            Descrizione = appuntamento.Descrizione,
            OrganizzatoreId = appuntamento.OrganizzatoreId,
            Titolo = appuntamento.Titolo,
            DataFine = appuntamento.DataFine,
            CategoriaId = appuntamento.Categoria.Id
        };
        _appuntamenti.Insert(app);
        var result = _appuntamenti.SaveChanges();
        if (result && appuntamento.Partecipanti.Count > 0)
        {
            var guidId = appuntamento.Partecipanti.Select(x => x.Id).ToList();
            await InsertPartecipazioni(guidId, app);
        }

        return ResponseFactory.CreateResponseFromResult(result);
    }

    public Response<List<AppuntamentoDaAccettareDto>> GetAppuntamentiDaAccettare(Guid userId)
    {
        if (userId == Guid.Empty)
            return ResponseFactory.CreateResponseFromResult<List<AppuntamentoDaAccettareDto>>(null, false,
                "L'id dell'utente non può essere vuoto");
        var partecipanti = GetAllPartecipantiAsDictionary();
        // prendo tutte le partecipazioni che non sono state eliminate 
        // che sono da accettare e che sono dalla data odierna in poi
        var appuntamenti = _partecipanti
            .Query(x => x.Utente.Id == userId && !x.IsDeleted
                                              && x.Evento.DataFine >= DateTime.Now &&
                                              x.StatoInvito == StatoInvito.Inviato && !x.IsDeleted)
            .Select(x => new AppuntamentoDaAccettareDto
            {
                DataFine = x.Evento.DataFine,
                DataInizio = x.Evento.DataInizio,
                Descrizione = x.Evento.Descrizione,
                Organizzatore = new UtenteDto(x.Utente.Id, x.Utente.Nome, x.Utente.Cognome, x.Utente.Email,
                    x.Utente.TipoUtente),
                Titolo = x.Evento.Titolo,
                Partecipanti = partecipanti[x.Id],
                Categoria = new CategoriaDto(x.Evento.Categoria.Descrizione, x.Evento.Categoria.Id,
                    x.Evento.Categoria.Color)
            })
            .ToList();
        return ResponseFactory.CreateResponseFromResult(appuntamenti);
    }

    private Dictionary<Guid, List<UtenteDto>> GetAllPartecipantiAsDictionary()
    {
        var partecipanti = _partecipanti
            .Get(filter: x => !x.IsDeleted, includes: x => x.Utente)
            .GroupBy(y => y.Id)
            .ToDictionary(
                group => group.Key,
                group => group
                    .Select(item =>
                        new UtenteDto(item.Utente.Id, item.Utente.Nome, item.Utente.Cognome, item.Utente.Email,
                            item.Utente.TipoUtente)).ToList());
        return partecipanti;
    }

    public Response<bool> AggiornaStatoInvito(Guid partecipazioneId, StatoInvito stato)
    {
        if (partecipazioneId == Guid.Empty)
            return ResponseFactory.CreateResponseFromResult(false, false,
                "L'id dell'utente non può essere vuoto");
        var partecipazione = _partecipanti
            .GetFirstOrDefault(x => x.Id == partecipazioneId);
        if (partecipazione.IsDeleted)
            return ResponseFactory.CreateResponseFromResult(false, false,
                "Partecipazione non presente.");
        partecipazione.StatoInvito = stato;
        _partecipanti.Update(partecipazione);
        var result = _partecipanti.SaveChanges();
        return ResponseFactory.CreateResponseFromResult(result);
    }

    private async Task InsertPartecipazioni(List<Guid> partecipantiId, Appuntamento app)
    {
        var email = new List<EmailToDto>();
        foreach (var partecipante in partecipantiId)
        {
            var utente = _utente.GetFirstOrDefault(x => x.Id == partecipante && !x.IsDeleted);
            var p = new Partecipante
            {
                EventoId = app.Id,
                StatoInvito = StatoInvito.Inviato,
                UtenteId = partecipante
            };
            _partecipanti.Insert(p);
            if (utente != null)
            {
                email.Add(new EmailToDto(utente.Nome, utente.Cognome, utente.Email));
            }
        }

        if (email.Count > 0)
        {
            var organizzatore = _utente.GetFirstOrDefault(x => x.Id == app.OrganizzatoreId && !x.IsDeleted);
            var result = await mailService.InviaEmailInvito(email, $"{organizzatore.Nome} {organizzatore.Cognome}",
                app.DataInizio);
            if (result)
            {
                _partecipanti.SaveChanges();
            }
        }
    }

    public async Task InviaReminderEmail()
    {
        var appuntamenti = _appuntamenti
            .Query(x => !x.IsDeleted
                        && x.DataInizio.Date == DateTime.Now.Date.AddDays(1)
                        && x.Partecipanti.Any(y => y.StatoInvito == StatoInvito.Accettato))
            // lo stato della partecipazione
            .Select(x => new
            {
                Organizzatore = $"{x.Organizzatore.Nome} ${x.Organizzatore.Cognome}",
                Data = x.DataInizio.ToString("D", new CultureInfo("it-IT")),
                Ora = x.DataInizio.ToString("HH:mm"),
                Emails = x.Partecipanti
                    .Select(y => new EmailToDto(y.Utente.Nome, y.Utente.Cognome, y.Utente.Email))
                    .ToList()
            })
            .Select(x =>
                new ReminderEmailDto(x.Organizzatore, x.Data, x.Ora, x.Emails))
            .ToList();
        await mailService.InviaEmailReminder(appuntamenti);
    }

    public Response<bool> UpdateAppuntamento(AppuntamentoDto appuntamento, Guid id)
    {
        ArgumentNullException.ThrowIfNull(appuntamento);
        if (id == Guid.Empty)
        {
            return ResponseFactory.CreateResponseFromResult(false, false,
                "L'id dell'appuntamento non può essere vuoto");
        }

        if (appuntamento.OrganizzatoreId == Guid.Empty)
        {
            return ResponseFactory.CreateResponseFromResult(false, false,
                "L'id dell'utente non può essere vuoto");
        }

        var app = _appuntamenti.GetById(id);
        if (app == null)
            return ResponseFactory.CreateResponseFromResult(false, false,
                "Non esiste un appuntamento con questo ids");
        app.Titolo = appuntamento.Titolo;
        app.Descrizione = appuntamento.Descrizione;
        app.DataFine = appuntamento.DataFine;
        app.DataInizio = appuntamento.DataInizio;
        app.OrganizzatoreId = appuntamento.OrganizzatoreId;
        _appuntamenti.Update(app);
        var result = _appuntamenti.SaveChanges();
        return ResponseFactory.CreateResponseFromResult(result);
    }
}