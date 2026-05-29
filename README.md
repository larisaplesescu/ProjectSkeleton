# 🧪 Potion Garden

Un joc de simulare și management al unui magazin alchimic, dezvoltat în **C#** folosind framework-ul **SDL2** pentru randarea grafică complexă. Scopul jocului este de a cultiva ingrediente magice, de a crafta poțiuni puternice și de a le vinde clienților care trec pragul magazinului pentru a strânge aur și a câștiga jocul!

---

## 🎮 Cum se joacă (Ghid Pas cu Pas)

Jocul se desfășoară în faze rapide și necesită atenție sporită pentru a menține clienții fericiți.

### Pasul 1: Pregătirea Solului și Plantarea
Grădina ta are **4 straturi de pământ** disponibile. Pentru a planta semințe folosești o singură combinație intuitivă:
* Ține apăsată tasta **`S`** (Seed) și apasă cifra stratului corespunzător pentru a planta instant:
  * `S + 1` ➡️ Plantează **Magic Carrot** 🥕 pe Stratul 1
  * `S + 2` ➡️ Plantează **Magic Tomato** 🍅 pe Stratul 2
  * `S + 3` ➡️ Plantează **Magic Corn** 🌽 pe Stratul 3
  * `S + 4` ➡️ Plantează **Magic Cabbage** 🥬 pe Stratul 4

### Pasul 2: Îngrijirea și Recoltarea
* **Udarea:** Plantele au nevoie de apă pentru a crește. Ține apăsată tasta **`W`** (Water) + cifra stratului (de exemplu: `W + 1`) pentru a uda planta.
* **Recoltarea:** Când bara de sub plantă devine verde și scrie **`RDY`**, planta este matură. Ține apăsată tasta **`H`** (Harvest) + cifra stratului (ex: `H + 3`) pentru a culege ingredientul magic în inventar.

### Pasul 3: Craftarea Poțiunilor (Alchemy)
Fiecare poțiune necesită **2/3/4 ingrediente identice** din stoc, în funcție de nivel. Pentru a combina ingredientele:
* Ține apăsată tasta **`C`** (Craft) și apasă cifra corespunzătoare poțiunii:
  * `C + 1` ➡️ **Speed Potion** (necesită 2x Magic Carrot)
  * `C + 2` ➡️ **Fire Potion** (necesită 2x Magic Tomato)
  * `C + 3` ➡️ **Sun Potion** (necesită 2x Magic Corn)
  * `C + 4` ➡️ **Nature Potion** (necesită 2x Magic Cabbage)
* Poțiunile similare vor fi grupate automat în inventar sub forma `Poțiune xCount`.

### Pasul 4: Servirea Clienților (Vânzarea)
* Clienții apar în partea de jos a ecranului și au o bară de răbdare roșie. Fiecare client cere o anumită poțiune.
* Pentru a-l servi pe primul client din listă (cel mai din stânga), apasă simplu tasta **`1`** (sau `2` pentru al doilea, `3` pentru al treilea). Dacă ai poțiunea cerută în stoc, aceasta va fi vândută, iar tu vei primi **Aur (Gold)** și **Scor**.

### Pasul 5: Upgrade-ul Magazinului 🏛️
* Adună aur de la clienți! Când atingi suma necesară, magazinul va face automat **Upgrade la Shop Level 2**, deblocând succesul și ducându-te mai aproape de victoria finală!

---

## 🛠️ Cerințe și Rulare Locală

Pentru a rula acest proiect pe calculatorul tău, ai nevoie de .NET SDK instalat.

1. Deschide un terminal în folderul proiectului:
   ```bash
   cd C:\Users\Laptop\ProjectSkeleton
