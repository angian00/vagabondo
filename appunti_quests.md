# Appunti

## Brainstorming
- esistono antagonisti/difficolta'/ modi di essere sconfitti?

### Azioni
- messaggi criptati


### Special destinations
Alcuni stati abilitano (solo per un turno?) delle destinazioni speciali, es. dungeon, rovine, templi, evidenziate visivamente in qualche modo.
In particolare, gli "overarching plot" consisterebbero in macchine a stati. Ogni stato implica una certa probabilita', condizionata magari alle caratteristiche della citta' corrente, di generare una data destinazione speciale funzionale all'avanzamento allo stato successivo.

## Overarching Plot
Da ChatGPT:

La ricerca dell'artefatto antico:
Passo 1: Durante il commercio in una città, il giocatore trova un antico manufatto in una bancarella.
Passo 2: Il giocatore può scegliere di indagare sull'origine dell'oggetto o proseguire il proprio viaggio.
Passo 3: Se sceglie di indagare, il giocatore ottiene informazioni su un antico tempio nelle vicinanze.
Passo 4: Nel tempio, il giocatore risolve un enigma per accedere a una stanza segreta contenente indizi sull'artefatto.
Passo 5: I frammenti di informazioni raccolti nelle varie città portano il giocatore a comprendere il vero scopo e la storia dell'artefatto.

La rivolta delle macchine:
Passo 1: Durante il commercio in una città, il giocatore nota strani malfunzionamenti nelle macchine locali.
Passo 2: Può scegliere di indagare sui malfunzionamenti o ignorarli e proseguire.
Passo 3: Se decide di indagare, scopre indizi che suggeriscono un sabotaggio.
Passo 4: Può scegliere di confrontare sospetti o cercare aiuto da parte di un gruppo di tecnici.
Passo 5: Unendo le informazioni raccolte nelle diverse città, il giocatore svela il complotto e può scegliere di fermare o favorire la rivolta delle macchine.

La maledizione del re scomparso:
Passo 1: In una città, il giocatore apprende della scomparsa misteriosa del re.
Passo 2: Può decidere di cercare indizi sulla scomparsa o proseguire con il proprio viaggio.
Passo 3: Se sceglie di indagare, scopre tracce che portano a un'antica rovina nelle vicinanze.
Passo 4: Esplorando la rovina, il giocatore trova frammenti di una maledizione e la storia del suo lancio.
Passo 5: Confrontando le informazioni raccolte nelle varie città, il giocatore svela la verità dietro la scomparsa del re e può decidere il suo destino.

Il segreto della città abbandonata:
Passo 1: Durante il viaggio, il giocatore scopre una città abbandonata e maledetta.
Passo 2: Può scegliere di esplorare la città o ignorare la sua presenza.
Passo 3: Se decide di esplorare, scopre che la città è stata abbandonata a causa di una tragedia.
Passo 4: Attraverso indizi trovati nella città, il giocatore impara la storia di ciò che è accaduto.
Passo 5: Unendo le informazioni raccolte nelle diverse città, il giocatore svela la verità dietro la catastrofe e la città abbandonata.

La profezia del nuovo sovrano:
Passo 1: Il giocatore apprende di una profezia che predice l'ascesa di un nuovo sovrano.
Passo 2: Può scegliere di indagare sulla profezia o ignorarla.
Passo 3: Se decide di indagare, scopre che la profezia è legata a un antico ordine segreto.
Passo 4: Attraverso incontri e indagini, il giocatore impara chi sia il prescelto dalla profezia e quali siano i suoi legami con il passato.
Passo 5: Unendo le informazioni raccolte nelle diverse città, il giocatore svela il significato della profezia e può decidere se favorire o contrastare il destino del prescelto.

In ciascuna trama, il giocatore avanza seguendo le azioni che decide di intraprendere durante il proprio viaggio, e le informazioni raccolte nelle varie città si accumulano per rivelare progressivamente la storia di fondo.

Ecco come il meccanismo potrebbe funzionare utilizzando frammenti di informazioni testuali:

La ricerca dell'artefatto antico:
Durante il viaggio, il giocatore raccoglie frammenti di informazioni, come "Un antico tempio si trova oltre il deserto", "L'artefatto è legato a una leggenda di poteri nascosti", "Gli antichi scritti parlano di un custode dell'artefatto".
Ogni volta che il giocatore visita una nuova città, può ottenere un nuovo frammento di informazione o reincontrare frammenti precedentemente raccolti, ma in un contesto diverso.
Alla fine, dopo aver raccolto abbastanza frammenti di informazioni, il giocatore riesce a mettere insieme i pezzi del puzzle e comprendere il vero scopo e la storia dell'artefatto.

Ad esempio, potrebbe essere:
Il giocatore trova una tavoletta di pietra antica con una scritta che suggerisce l'esistenza di un tempio oltre il deserto.
In un'altra città, ascolta un racconto popolare che menziona l'artefatto e le sue leggendarie capacità.
Durante un'altra avventura, il giocatore trova un diario di un esploratore che descrive un custode dell'artefatto che protegge il tempio.
In questo modo, ogni frammento di informazione contribuisce a costruire la comprensione complessiva dell'obiettivo principale del giocatore nel trovare e comprendere l'artefatto antico.


### QuestFragments
Al massimo una Quest attiva ad un dato istante.
I frammenti di info su una quest sono annegati come parte delle Memories, magari con un colore diverso.

QuestFragment 1
title: The Abandoned Church
description: A local farmer told you that somewhere in a neighbouring valley lay the ruins an old church, abandoned a century ago after a plague.

QuestFragment 2
title: An Artifact of Yore
description: i racconti popolari della zona menzionano un antico artefatto ormai smarrito. l'artefatto e' di pietra verde ricoperta di muschio. secondo alcuni porta bene, secondo altri male.

QuestFragment 3
title: The Church Warden
description: hai trovato il diario del guardiano della chiesa. vi si dice che la chiesa gode di una protezione dagli influssi maligni che allignano nel bosco


State machine:
QF 1 --> QF 2 --> QF3 --> <the curch> --> recupero artefatto

Logica generazione SpecialActions:
- al tempo t0: creazione procedurale Quest; init quest state

- quando si generano le azioni: prob(special action) = p. if (randomize < p) --> generate special action (quest state); update quest state


## Resources
Corpora
https://github.com/dariusk/corpora/tree/master
