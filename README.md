```markdown
# Projekt Zaliczeniowy - PRO 1 🎓

[![Uczelnia](https://img.shields.io/badge/PJATK-Warszawa-blue.svg)](https://pja.edu.pl/)
[![Przedmiot](https://img.shields.io/badge/Przedmiot-PRO_1-orange.svg)]()
[![Wydział](https://img.shields.io/badge/Wydział-XRG-green.svg)]()
[![Silnik](https://img.shields.io/badge/Engine-Unity_2D-blueviolet.svg?logo=unity)](https://unity.com/)
[![Język](https://img.shields.io/badge/Lang-C%23-green.svg?logo=csharp)](https://docs.microsoft.com/en-us/dotnet/csharp/)

Projekt przygotowany w ramach zaliczenia przedmiotu **PRO 1** (Programowanie 1) na **Polsko-Japońskiej Akademii Technik Komputerowych w Warszawie**, na wydziale **XRG**.

Repozytorium: [2025_13c_01_6week](https://github.com/VeeturN/2025_13c_01_6week)

---

## 👥 Zespół Projektowy (Grupa 13c_01)

Projekt realizowany w 4-osobowym zespole. Poniżej znajduje się podział obowiązków (przykład):

* **@VeeturN** – Logika gracza, system umiejętności (dash, double jump).
* **[Nick 2]** – AI Przeciwników (Melee, Range), system walki.
* **[Nick 3]** – System Save/Load, rozbudowane UI/GUI, dialogi.
* **[Nick 4]** – Tilemapa, trampoliny, platformy ruszające/znikające, animacje.

---

## 📝 Opis Projektu

Głównym celem projektu była implementacja funkcjonalnej, dwuwymiarowej gry platformowej przy użyciu silnika Unity oraz języka C#. Gra łączy w sobie klasyczne elementy platformowe z systemem walki i rozwoju postaci.

### Główne mechaniki i funkcjonalności:

1.  **System Ruchu i Umiejętności Gracza:**
    * Podwójny skok (**Double Jump**).
    * Szybki doskok (**Dash**).
    * Interakcja z otoczeniem (trampoliny, platformy).
2.  **System Walki (Combat):**
    * Ataki zwarciowe (**Melee**).
    * Ataki dystansowe (**Range**).
3.  **AI Przeciwników:**
    * Proste algorytmy zachowania dla przeciwników walczących w zwarciu i na dystans.
    * Różne wzorce ataków.
4.  **Otoczenie i Świat Gry:**
    * Wykorzystanie systemu **Tilemap** do budowy poziomów.
    * Różnorodne typy platform: ruszające się, znikające.
5.  **Zasoby i Postęp:**
    * System znajdziek (**Collectibles**).
    * Kompletny **Save System** (zapis/odczyt stanu gry).
6.  **Interfejs i Fabuła:**
    * Rozbudowane **UI/GUI** (zdrowie, wynik, menu).
    * System **Dialogów**.
    * Spójne **Animacje** postaci i otoczenia.

## 🚀 Wymagania i Uruchomienie

### Wymagania:
* Unity Editor (wersja rekomendowana: 2022.3 LTS lub nowsza).
* Git.

### Klonowanie i Otwieranie:
1.  **Sklonuj repozytorium:**
    ```bash
    git clone [https://github.com/VeeturN/2025_13c_01_6week.git](https://github.com/VeeturN/2025_13c_01_6week.git)
    ```
2.  **Otwórz Unity Hub.**
3.  Kliknij **Add** -> **Add project from disk**.
4.  Wskaż folder, do którego sklonowałeś repozytorium.
5.  Unity Hub automatycznie wykryje wersję i pobierze wymagane pakiety.
