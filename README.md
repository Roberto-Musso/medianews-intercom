# MediaNews Intercom - 8 Channel Professional Audio System

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4)](https://dotnet.microsoft.com/)
[![Platform](https://img.shields.io/badge/Platform-Windows-0078D4)](https://www.microsoft.com/windows)

Professional 8-channel audio intercom system with bidirectional communication between standard WDM microphone/speaker and 8 channels of a professional ASIO audio interface, featuring remote web control.

![Version](https://img.shields.io/badge/version-1.1.0-blue)

## Caratteristiche Principali

### Sistema Intercom Completo
- **8 Canali Indipendenti**: Ogni canale ha routing completo, livelli input/output, Talk e Listen individuali
- **Talk Buttons Individuali**: Ogni canale può inviare il microfono alla propria uscita ASIO indipendentemente
- **Listen Buttons Individuali**: Ogni canale può essere ascoltato indipendentemente
- **Mixer Audio Integrato**: Combina automaticamente i canali Listen attivi
- **Controllo Granulare**: Attiva/disattiva Talk e Listen per canale specifico

### Percorsi Audio

**TALK PATH** (Microfono → ASIO):
```
[Microfono WDM] → [Talk Button Canale N] → [Controllo Output Level Canale N] → [ASIO Output Channel N]
```
Ogni canale decide indipendentemente se ricevere il microfono

**LISTEN PATH** (ASIO → Altoparlante):
```
[ASIO Input Channel N] → [Controllo Input Level Canale N] → [Listen Button Canale N] → [Audio Mixer] → [Altoparlante WDM]
```
Più canali Listen possono essere attivi contemporaneamente (mixer automatico)

### Controlli Audio
- **Input Level per canale**: 0-130 (0 = silenzio, 100 = unità, 130 = +30% amplificazione)
- **Output Level per canale**: 0-130 (stesso range)
- **Mixing automatico**: I canali Listen attivi vengono mixati insieme senza clipping

### Interfaccia Utente
- **8 Talk Buttons**: Uno per canale, diventano ROSSI quando attivi
- **8 Listen Buttons**: Uno per canale, diventano AZZURRI quando attivi
- **Channel Strips**: Disposizione 2x4 con tutti i controlli visibili per canale
- **16 Slider Livelli**: Input Level e Output Level per ogni canale (0-130)
- **Log Console**: Monitoraggio in tempo reale di tutte le operazioni
- **Visual Feedback**: Codifica colori per stato immediato (Rosso=Talk, Azzurro=Listen, Grigio=Off)

## Requisiti

### Software
- Windows 10/11
- .NET 8.0 Runtime o superiore
- Driver ASIO installato (es. ASIO4ALL, FlexASIO, o driver della tua interfaccia audio professionale)

### Hardware
- Microfono (dispositivo WDM/WASAPI di input)
- Altoparlante/Cuffie (dispositivo WDM/WASAPI di output)
- Interfaccia audio ASIO con almeno 8 input e 8 output

## Download e Installazione

### Opzione 1: Installer Windows (Raccomandato)

1. Scarica l'ultimo installer dalla [pagina Releases](https://github.com/robertomusso/medianews-intercom/releases)
2. Esegui `MediaNews-Intercom-Setup-v1.1.0.exe`
3. Segui la procedura guidata di installazione
4. L'installer include:
   - ✅ Applicazione completa
   - ✅ .NET 8.0 Runtime (se necessario)
   - ✅ Icona sul desktop
   - ✅ Collegamento nel menu Start
   - ✅ Script per abilitare accesso web di rete

### Opzione 2: Esecuzione Portable (senza installazione)

1. Scarica il pacchetto ZIP dalla [pagina Releases](https://github.com/robertomusso/medianews-intercom/releases)
2. Estrai in una cartella
3. Esegui `MediaNews-Intercom.exe`

### Opzione 3: Compilazione da sorgente

```bash
# Clona il repository
git clone https://github.com/robertomusso/medianews-intercom.git
cd medianews-intercom

# Compila il progetto
dotnet build

# Esegui l'applicazione
dotnet run --project AudioWdmToAsio/AudioWdmToAsio.csproj
```

## Guida Rapida all'Utilizzo

### 1. Configurazione Iniziale

1. **Avvia l'applicazione**
2. **Clicca "Settings"** per aprire il pannello di configurazione
3. **Configura i dispositivi**:
   - Seleziona il microfono (WDM Input)
   - Seleziona l'altoparlante (WDM Output)
   - Seleziona l'interfaccia ASIO
   - Regola il buffer (consigliato: 100ms per iniziare)

4. **Configura il mapping dei canali ASIO**:
   - Per ogni canale software (1-8), scegli quale canale fisico ASIO usare
   - ASIO Input Channel: Da dove ricevere l'audio
   - ASIO Output Channel: Dove inviare l'audio
   - Default: Canale 1 → ASIO Ch 0, Canale 2 → ASIO Ch 1, etc.

5. **Clicca "OK"** per salvare le impostazioni

### 2. Avvio del Sistema

1. **Clicca "START"** nella barra inferiore
2. Il sistema inizia lo streaming audio
3. Lo status diventa "Streaming Active" (verde)
4. Il sistema è ora pronto per Talk e Listen

### 3. Utilizzo del Talk (Inviare Audio)

1. **Clicca "TALK"** sul canale a cui vuoi inviare il microfono
2. Il pulsante diventa ROSSO
3. Il tuo microfono viene ora inviato **solo** a quel canale ASIO
4. **Regola Output Level** di quel canale per controllare il volume inviato
5. **Attiva più TALK** su altri canali per inviare a più destinazioni contemporaneamente
6. **Clicca "TALK" di nuovo** sul canale per disattivarlo (diventa grigio)

### 4. Utilizzo del Listen (Ricevere Audio)

1. **Clicca "LISTEN"** sul canale che vuoi ascoltare
2. Il pulsante diventa AZZURRO
3. L'audio da quel canale ASIO viene inviato al tuo altoparlante
4. **Regola Input Level** per controllare il volume ricevuto
5. **Attiva più canali Listen** per ascoltare più sorgenti contemporaneamente
   - Il mixer le combinerà automaticamente
6. **Clicca "LISTEN" di nuovo** per disattivare quel canale

### 5. Regolazione dei Livelli

**Input Level** (0-130):
- Controlla il volume dell'audio in arrivo da ASIO (per Listen)
- 0 = Silenzio totale
- 100 = Volume normale (default)
- 130 = Amplificazione +30%

**Output Level** (0-130):
- Controlla il volume dell'audio in uscita verso ASIO (per Talk)
- Stesso range di Input Level

### 6. Arresto del Sistema

1. **Clicca "STOP"** nella barra inferiore
2. Tutti i canali vengono disattivati
3. Talk e Listen vengono resettati
4. Il sistema torna in stato Idle

## Casi d'Uso Tipici

### Scenario 1: Studio di Registrazione
- **Talk**: Il produttore parla selettivamente a musicisti specifici (canale 1=batterista, 2=chitarrista, etc.)
- **Listen**: Il produttore ascolta selettivamente singoli musicisti o mix di più musicisti

### Scenario 2: Broadcast/Podcast
- **Talk**: L'host comunica selettivamente con ospiti specifici (canale privato)
- **Listen**: L'host ascolta uno o più ospiti contemporaneamente (mixer automatico)

### Scenario 3: Teatro/Live Sound
- **Talk**: Il direttore audio comunica con postazioni specifiche (Talk solo palco, solo luci, etc.)
- **Listen**: Il direttore ascolta feedback da posizioni selettive o tutte insieme

### Scenario 4: Conferenze Multi-Punto
- **Talk**: Moderatore invia audio a sale specifiche (Talk sala 1, 3, 5 = solo quelle ricevono)
- **Listen**: Moderatore monitora domande da sale selezionate

## Architettura Tecnica

### Stack Tecnologico
- **Framework**: .NET 8.0 Windows Forms
- **Libreria Audio**: NAudio 2.2.1
  - NAudio.Asio (gestione driver ASIO)
  - NAudio.Wasapi (cattura/riproduzione audio Windows)
  - NAudio.Core (elaborazione audio)

### Componenti Principali

#### 1. IntercomSettings
Configurazione globale che contiene:
- Dispositivi WDM selezionati
- Driver ASIO selezionato
- 8 oggetti IntercomChannel
- Stato Talk e streaming

#### 2. IntercomChannel
Ogni canale contiene:
- Numero canale (1-8)
- Stato Talk (attivo/inattivo)
- Stato Listen (attivo/inattivo)
- Livelli Input e Output (0-130)
- Mapping canali ASIO fisici (input e output)

#### 3. Audio Engine
- **WasapiCapture**: Cattura dal microfono
- **AsioOut**: Gestisce I/O ASIO bidirezionale
- **WasapiOut**: Riproduzione su altoparlante
- **BufferedWaveProvider**: Buffers per gestire latenza

### Flusso Audio Dettagliato

**Talk Path**:
1. Microfono cattura audio (WasapiCapture)
2. Audio va in `talkBuffer` solo se almeno un Talk è attivo
3. ASIO AudioAvailable legge da `talkBuffer`
4. Distribuisce **solo** ai canali con Talk attivo, con gain individuale
5. Canali senza Talk attivo ricevono silenzio
6. Hardware ASIO riceve audio sui canali selezionati

**Listen Path**:
1. ASIO AudioAvailable riceve da canali input
2. Per ogni canale con Listen attivo:
   - Legge dal buffer ASIO
   - Applica gain Input Level
   - Aggiunge al mixer
3. Mixer normalizza ed evita clipping
4. Output va a `speakerBuffer`
5. WasapiOut riproduce sull'altoparlante

### Gestione Buffer
- **talkBuffer**: Dimensione configurabile (10-500ms)
- **speakerBuffer**: Stessa dimensione, formato Float32 stereo
- **DiscardOnBufferOverflow**: Previene blocchi
- Thread-safe per operazioni real-time

## Struttura del Progetto

```
AudioWdmToAsio/
├── IntercomChannel.cs        # Modello dati singolo canale
├── IntercomSettings.cs       # Configurazione globale
├── Form1.cs                  # Logica principale intercom
├── Form1.Designer.cs         # UI principale (8 channel strips)
├── SettingsForm.cs           # Logica pannello impostazioni
├── SettingsForm.Designer.cs  # UI pannello impostazioni
├── Program.cs                # Entry point applicazione
└── AudioWdmToAsio.csproj     # Configurazione progetto
```

## Risoluzione Problemi

### "Nessun driver ASIO trovato"
- Installa un driver ASIO:
  - **ASIO4ALL** (gratuito): http://www.asio4all.org/
  - **FlexASIO** (open source): https://github.com/dechamps/FlexASIO
  - Driver proprietario della tua interfaccia audio

### "Please configure devices in Settings first"
- Apri Settings e configura tutti e tre i dispositivi:
  - Microphone (WDM)
  - Speaker (WDM)
  - ASIO Device

### Alta latenza / Echo
- Riduci il buffer size nelle impostazioni (prova 50ms o meno)
- Chiudi altre applicazioni audio
- Verifica le impostazioni del pannello ASIO del driver

### Audio interrotto / Dropouts
- Aumenta il buffer size (prova 150ms o più)
- Chiudi applicazioni in background
- Verifica che il driver ASIO sia configurato correttamente
- Controlla le prestazioni della CPU

### Canale non funziona
- Verifica il mapping dei canali ASIO in Settings
- Controlla che il canale ASIO fisico sia nei limiti del driver
- Verifica i livelli Input/Output (potrebbero essere a 0)

### Volume troppo basso/alto
- Regola Input Level per Listen (0-130)
- Regola Output Level per Talk (0-130)
- Default 100 = volume unitario
- Valori > 100 amplificano ma possono causare distorsione

### "Error starting streaming"
- Verifica che nessun'altra applicazione stia usando il driver ASIO
- Riavvia l'applicazione
- Controlla il log console per dettagli specifici
- Prova un buffer size diverso

## Limitazioni Note

- Il sistema usa un singolo driver ASIO per tutti gli 8 canali
- Alcuni driver ASIO hanno requisiti specifici di buffer size
- La conversione di sample rate non è implementata (usa il rate del microfono)
- Maximum 8 canali (può essere esteso modificando il codice)
- Listen mixer supporta fino a 8 canali simultanei

## Funzionalità Future Possibili

- [ ] VU meters per visualizzare livelli audio in tempo reale
- [ ] Preset salvabili/caricabili per configurazioni diverse
- [ ] Talkback con sidetone (ascoltarsi mentre si parla)
- [ ] Registrazione delle sessioni intercom
- [ ] Supporto per più di 8 canali
- [ ] Effetti audio (EQ, compressore, gate, etc.)
- [ ] Hotkey/shortcuts per Talk e Listen
- [ ] MIDI control per controllo esterno
- [ ] Network streaming per intercom remoto

## Best Practices

### Per Bassa Latenza
1. Usa buffer piccolo (30-100ms)
2. Chiudi applicazioni non necessarie
3. Usa driver ASIO nativi invece di emulati
4. Monitora la CPU e non superare 70%

### Per Stabilità
1. Usa buffer medio-grande (100-200ms)
2. Testa la configurazione prima dell'uso live
3. Salva configurazioni funzionanti
4. Fai un soundcheck prima di eventi importanti

### Per Qualità Audio
1. Usa interfacce audio di qualità
2. Mantieni livelli tra 80-120 per evitare distorsione
3. Usa microfoni e altoparlanti di qualità
4. Evita feedback loops (non ascoltare Talk mentre è attivo)

## Sviluppo

### Dipendenze NuGet
```xml
<PackageReference Include="NAudio" Version="2.2.1" />
<PackageReference Include="NAudio.Asio" Version="2.2.1" />
```

### Build
```bash
dotnet build AudioWdmToAsio.sln
```

### Debug
Apri `AudioWdmToAsio.sln` in Visual Studio 2022 o Visual Studio Code per debugging dettagliato.

### Estendere il Sistema
Per aggiungere più canali:
1. Modifica `IntercomSettings` per creare più canali
2. Modifica `Form1.Designer.cs` CreateChannelStrips() per crearli nell'UI
3. La logica audio è già predisposta per N canali

## Licenza

Questo progetto è rilasciato sotto licenza MIT. Vedi il file [LICENSE](LICENSE) per i dettagli completi.

Utilizzabile liberamente per scopi educativi e commerciali.

## Contributing

Contributi, issues e feature requests sono benvenuti!

1. Fork il progetto
2. Crea un branch per la tua feature (`git checkout -b feature/AmazingFeature`)
3. Commit le modifiche (`git commit -m 'Add some AmazingFeature'`)
4. Push al branch (`git push origin feature/AmazingFeature`)
5. Apri una Pull Request

## Crediti

- **NAudio**: https://github.com/naudio/NAudio (Mark Heath)
- **ASIO SDK**: Steinberg Media Technologies GmbH
- **Sviluppato con**: Claude Code (Anthropic)

## Supporto

Per problemi, domande o suggerimenti:
1. Consulta la sezione "Risoluzione Problemi"
2. Controlla il log console per errori dettagliati
3. Verifica la configurazione ASIO tramite il pannello del driver
4. Prova con buffer sizes diversi

## Changelog

### Versione 1.1.0 (Corrente) - Remote Control & UX Improvements
- 🌐 **Web Remote Control**: Interfaccia web completa per controllo remoto
  - Accesso da qualsiasi dispositivo sulla rete locale
  - Controllo Talk/Listen per tutti i canali
  - Regolazione livelli input/output da browser
  - VU Meter in tempo reale
  - Responsive design per mobile/tablet
- 🖱️ **Miglioramento Controlli Rotary**:
  - Mouse capture durante drag (funzionano anche fuori dal box del canale)
  - Doppio click per input numerico diretto da tastiera
  - Conferma con INVIO, annulla con ESC
- 🔧 **Setup di Rete Automatico**:
  - Script ENABLE_NETWORK_ACCESS.bat per configurazione firewall
  - Messaggi diagnostici migliorati
  - Auto-detection IP locale
- 📦 **Installer Professionale**: Setup Windows con tutte le dipendenze

### Versione 1.0.0 - Intercom System
- **Sistema Intercom a 8 Canali**: Completo con Talk e Listen individuali
- **8 Talk Buttons**: Controllo individuale per invio microfono per canale
- **8 Listen Buttons**: Controllo individuale per ricezione canali
- **Controlli Livello**: Input e Output Level per ogni canale (0-130)
- **Audio Mixer**: Combina automaticamente canali Listen attivi
- **Pannello Settings**: Configurazione completa dispositivi e mapping canali
- **UI Professionale**: Design dark theme con feedback visivo immediato
- **Channel Mapping**: Routing flessibile tra canali software e hardware ASIO
- **Real-time Logging**: Console integrata per monitoring sistema
- **VU Meters**: Visualizzazione livelli audio in tempo reale

---

**MediaNews Intercom v1.1** - Professional 8-Channel Communication Platform with Remote Web Control
