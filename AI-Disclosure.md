# AI Usage Disclosure

Acest document descrie modul în care inteligența artificială (Google Gemini) a fost utilizată în procesul de dezvoltare al proiectului **Potion Garden** ca asistent de programare.

## Logica și Implementarea Manuală (Scris de Student)
Cea mai mare parte a arhitecturii funcționale a fost scrisă, testată și integrată manual pentru a se mula pe scheletul de cod nativ oferit la laborator:
* **Integrarea Input-ului (Program.cs):** Maparea tastelor combinate pentru straturi multiple (`S + cifră` pentru plantare, `W + cifră` pentru udare, `C + cifră` pentru craft).
* **Fluxul Jocului și Mecanicile Core:** Regulile de business pentru stările plantelor (Ready, Watered), vânzarea către clienți, deducerea aurului și logica de progresie pentru nivelele magazinului.
* **Depanarea și Corelarea:** Integrarea finală cu metodele native din `GameRenderer` și `Inventory`.

---

## Cum a fost utilizat asistentul AI
Inteligența artificială a fost folosită punctual pentru optimizări algoritmice, refactorizare de structuri de date și salvare persistentă.

### 1. Sugestii și Structurare de Cod (Chat-based)
* **Gruparea Poțiunilor în Inventar:** Suport în scrierea algoritmului din `GameLogic` care curăță și reconstruiește periodic lista vizuală a poțiunilor în formatul compact `xCount` (evitând aglomerarea interfeței grafice și randarea pe rânduri separate).
* **Mecanica de Dificultate Dinamică:** Asistență la scrierea formulei din metoda `CraftPotion` pentru determinarea numărului de plante necesare în funcție de nivelul magazinului (`ShopLevel`).

### 2. Persistența Datelor & I/O
* Generarea metodelor sigure de citire/scriere pe disk (folosind `File.Exists` și `File.WriteAllText`) pentru implementarea sistemului de **High Score permanent**, reținut în fișierul extern `highscore.txt`.

### 3. Logica de Debugging (Rubber-ducking)
* Rezolvarea erorilor de compilare (de tipul *CS8130 Cannot infer type* sau *CS1061 missing definition for Items*) apărute la interfațarea dinamică (`dynamic`) cu tipul de date rigid `Inventory<T>` din cadrul grafic.

## Regiuni de Cod Complet Generate sau Refactorizate de AI
* `GameLogic.cs`: Metoda `RebuildPotionsInStockVisual()` și blocul de verificare `CheckAndSaveHighScore()`.
* `Program.cs`: Logica de interpolare dinamică pentru generarea notificărilor text de eroare (`$"Need {required} Magic..."`).