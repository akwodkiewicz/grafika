# Edytor wielokątów

#### Autor: Andrzej Wódkiewicz

##### Instrukcja obsługi:
Aplikacja działa w dwóch trybach: *tworzenia* i *edycji*.

W *trybie tworzenia* można dodawać do obszaru głównego aplikacji maksymalnie 20 wierzchołków, tworząc w ten sposób wielokąt.
Wierzchołki dodaje się, klikając lewy przycisk myszy [LPM].
Tworzenie wielokąta można ukończyć na 2 sposoby:
- umieszczając kolejny z wierzchołków w miejscu wierzchołka startowego
- wciskając klawisz [Enter]

Po utworzeniu wielokąta aplikacja przechodzi do *trybu edycji*.

W *trybie edycji* można dokonywać następujących akcji:
```
                        Przesuwanie wierzchołka : przytrzymanie [LPM] i ruch myszą
                           Przesuwanie krawędzi : przytrzymanie [LPM] i ruch myszą
Przełączanie trybu przesuwania całego wielokąta : [A]
                        Reset obszaru głównego  : [R]
                           Usuwanie wierzchołka : [PPM] (na wierzchołku) -> Delete
        Dodawanie wierzchołka w środku krawędzi : [PPM] (na krawędzi) -> Add Vertex
          Blokowanie krawędzi w pionie/poziomie : [PPM] (na krawędzi) -> Lock Vertically/Lock Horizontally
                 Ustawianie kąta na wierzchołku : [PPM] (na wierzchołku) -> Lock Angle
       Usunięcie blokady z krawędzi/wierzchołka : [PPM] -> Clear Lock
```

##### Algorytm relacji:
Sprawdzenie relacji następuje podczas następujących operacji:
- Blokowanie krawędzi w poziomie
- Blokowanie krawędzi w pionie
- Blokowanie kąta w wierzchołku
- Poruszanie krawędzią
- Poruszanie wierzchołkiem

Dla pierwszych trzech operacji, przeprowadzane jest sprawdzenie *bezpośrednio bliskich* krawędzi i wierzchołków:
- dla operacji 1. i 2. sprawdzane są maksymalnie 4 wierzchołki i 2 krawędzie,
- dla 3. operacji sprawdzane są maksymalnie 4 wierzchołki i 4 krawędzie.

Dla dwóch ostatnich operacji (poruszanie krawędzią wywołuje dwukrotnie funkcję poruszania wierzchołkiem) wykonywany jest inny algorytm:

1. Zaczynając od wierzchołka startowego, wybieramy kolejne w stronę przeciwną do ruchu wskazówek zegara,
2. Sprawdzamy, czy obecny wierzchołek LUB odcinek wychodzący z tego wierzchołka LUB drugi koniec krawędzi zawiera jakieś obostrzenie
3. Jeśli tak, dodajemy aktualnie rozpatrywany wierzchołek do listy wierzchołków do poprawienia
4. Sprawdzamy wierzchołki do momentu, aż któryś z nich nie będzie musiał być poprawiony LUB dojdziemy do wierzchołka startowego
5. Następnie powtarzamy punkty 2-4 od wierzchołka startowego, poruszając się zgodnie do ruchu wskazówek zegara.
6. Przesuwamy wierzchołek startowy o dx, dy (zgodne z akcją użytkownika), a wraz z nim przesuwamy wszystkie wierzchołki znajdujące się na liście, również o dx, dy.
 