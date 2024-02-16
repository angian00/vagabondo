# TODO
- implementare item scarcity

- other improvements
	- ui: +/- nutrition feedback
	- shop inventorySize varies with town size


- brainstorming:
	- non-food items:
		- flowers; usage:
			- as gifts
			- in healing recipes
			+ as ingredients in higher-level decorations
		- medicine ingredients
		- mushrooms and wild berries

		- household items
			- used for crafting and trading
		- jewelry
		- raw materials
			- metal
			- wood
			- gems

	- memorabilia
		- trinkets (unique, non-tradable items)
		? permanent modifications: scars, tattoos, ...


- GameItem refactoring:
	- make other game item categories and generators
		- fiori
		- medicinal herbs
		+ books
	- tidy up categories
	- tidy up merchandise generator

- uniform grammar/ingredient pluralization/refExpansion


- town description grammar
	- bugfixing: -ing con r finale (capturring, mirrorring)

	- generalizzare sentences:
		- children -> people
		- church -> public building
		- crops in the wind -> aggiungere location rural; altri landscape e condizioni atmosferiche

	- altre sentences:
		- citta' grandi
		- clima cupo

	- loggare opzionalmente regole utilizzate per produrre una frase

	+ implementazione grammar: tenere traccia delle frasi gia' usate, con timestamp
	+ parametrizzare per bioma, stagione, stato economico e militare

- town and event actions
	+ fare qualcosa quando vengono generate 0 actions
	+ logica disabilitazione destinations e hints
	+ town traits: randomize on dominion traits/town sizes
	+ riuso domini non saturati

- data
	- improve action texts

- write quest generation grammar

- actions
	- GiveItem action flow
	- azioni complesse (ChoiceTree)
	+ generazione random wild animals (by biome)

+ autoplay mode to test balance

+ brainstorming:
	- religioni/soprannaturale
	- influenza del passare del tempo:
		- raccolta erbe con la luna nuova
		- cambia l'effetto dei biomi con le stagioni

+ grammar:
	+ [optional] syntax


## FIXME


## Errands
- provare American town generator
https://github.com/james-owen-ryan/talktown
