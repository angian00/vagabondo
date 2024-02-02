# Appunti: procedural generation

### Town
- edifici: correlati a size

### items
- quality
- base value deve essere compatibile con min e max dipendenti dalla causa/persona che genera l'item

- per food, base value = f(ingredients, preparation)

### food ingredient
- name
- category (valore unico, sottoinsiemi disgiunti)
- base value
- biomi (dictionary biome -> weight, o lista biomi compatibli)
+ "elemento" (caldo/freddo, ...)
? altri tag

### food item
- nome
- categoria (una o piu')
- lista ingredienti (compresi spezie e grassi)
- qualita' (dell'esecuzione della preparazione)
- valore base (calcolato da ingredienti, preparazione, qualita')
- preparazione (zero o una, forse piu'?);  nulla in caso di ingredienti grezzi
- ricetta di origine, nota o meno; al limite nulla in caso di ingredienti grezzi
+ "elementi" (caldo/freddo, ...)
+ deperibilita'

### generazione food item
- generare pool di ingredienti per il contesto di origine (ad es. interlocutore)
- combinare ingredienti a partire da templates (tipo grammar)
+ if (combinazione valida) ok, else repeat


### trinket
- nome
- categoria
- materiale
- trattamento
- qualita' manifattura
- rarita'
+ ornamenti (gemme incastonate, ...)
+ dedica a personaggi di rilievo, divinita'
+ identita' artefice
