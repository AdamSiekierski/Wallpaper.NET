# Wallpaper.NET Readme file
## Dobra, będę pisał po Polsku

Aplikacja oparta na .NET, a dokładniej na WPF. W zależności od daty, zmienia naszą tapetę, tj. w grudniu na świąteczną, przed Wielkanocą na... wielkanocną. W pozostałe daty pozostawia normalnie.

### Dla osób czytających jako dev:

Klasa okna głownego to oczywiście MainWindow. Klasa WallpaperControl odpowiada za zarządzanie procesami związanymi za backendem, czyt. ustawianie tapety. Potem mamy TrayClass - ona odpowiada za ikonkę w Taskbarze i za chowanie się aplikacji. Jest jeszcze EasterCalculator, on oblicza datę wielkanocy w danym roku.
