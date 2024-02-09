# TODO
- town and event actions
	- Church
	- Monastery
	- TownHall
	- Tavern
	- Library

	+ generazione wild animals (by biome)

- data
	- fill in building generation metadata
	- improve quest action templates

- trading
	- filtrare tipi di item vendibili per shop
	+ prezzi diversi di vendita e acquisto
	? alcuni "shop" (es. farm) vendono tipi diversi da quelli che comprano
	
- refactoring:
		- make TradableItem as class, and food/foodIngredients/herbs as its Components

- food generation
	- aggiungere frequency a foodItemTemplates
	- aggiungere templates per food raw

- reorganize traveler stats

- ui
	- skip ActionResult for shops
	- fix overflow testo e/o aggiungere tooltip con testo completo
	+ rendere scrollabile gli inventory

- manage defeat

- town description grammar
	- parametrizzare per bioma, stagione, stato economico e militare

	- generalizzare sentences:
		- children -> people
		- church -> public building
		- crops in the wind -> aggiungere location rural; altri landscape e condizioni atmosferiche

	- altre sentences:
		- citta' grandi
		- clima cupo

	- implementazione grammar: tenere traccia delle frasi gia' usate, con timestamp

- town
	- generare hints
	+ improve dominion permanence (town counter?)
	+ correlare town size a dominion size

- ui
	- evidenziare cambio di dominio


- brainstorming:
	- healing system

	- altre azioni
		-> cercare azioni tipiche libro-game
		
		- Asking Around
		- Sightsee
		- Find Person or Work
		- Rest
		- Gambling

		- help out
		        - lost animal
		        - injured man

		- leisure time
		        - festival
		        - performance
		        - playing darts in the tavern

		- gossip
		        - telling story around a fire
		        - gossip of adultery
		        - offer drink in the tavern

		- bambini che giocano

		- outsider npcs
		        - traveling priest/preacher
		        - fortune-teller
		        - exotic merchant/caravan
		        - beggar
		        - crazy man

		- exotic vendors
		        - animal seller
		        - flower seller

		- cult


- food generator:
	- elenchi
		+ fiori
		+ medicinal herbs
		- almonds
		- correggere mulled wine

	- armonizzare espansione nomi con rich grammar


+ brainstorming:
	- religioni/soprannaturale
	- influenza del passare del tempo:
		- raccolta erbe con la luna nuova
		- cambia l'effetto dei biomi con le stagioni

+ logica valutazione prezzi
+ logica generazione azioni

+ grammar:
	+ [optional] syntax


## FIXME
- differenziare azioni quest da foodgift


## Errands
- provare trial copilot
https://github.com/github-copilot/signup

- provare American town generator
https://github.com/james-owen-ryan/talktown
