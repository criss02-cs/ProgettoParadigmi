
# Calendar Application

Il progetto Calendar consiste in un applicativo mobile per la gestione di appuntamenti/eventi.

Per clonare il progetto eseguire il comando:
```git 
git clone https://github.com/criss02-cs/ProgettoParadigmi
```

All'interno del repository è presente il backup del database SQLServer, per ripristinarlo seguire il tutorial presente in questo [link](https://www.c-sharpcorner.com/article/how-to-import-or-restore-bacpac-file-from-ssms/).


Una volta ripristinato il database si potrà avviare l'applicativo, per farlo ci sono 2 possibilità:

- Avviarlo da Visual Studio/Rider
- Avviarlo da linea di comando

## Avviare con visual studio/rider

Per lanciare il progetto bisogna aprire la soluzione con un IDE come Visual Studio 2022 o Rider. Selezionare come progetto di avvio il progetto ProgettoParadigmi.Api, e cliccare esegui (o anche F5) per avviarlo in modalità debug.

## Avviare da linea di comando

Per avviare il progetto da linea di comando bisogna posizionarsi con il terminale nella cartella del progetto ProgettoParadigmi.Api. Poi eseguire il comando:
```cmd
dotnet run
```
L'applicativo sarà lanciato in localhost alla porta 7297, per poter vedere le API in con swagger posizionarsi al seguente percorso: https://localhost:7297/swagger/index.html
