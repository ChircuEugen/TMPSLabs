Este clasa "Car" care reprezinta o masina cu anumiti parametri: nume, model, data publicarii, numarul de roti, tiipul de alimentare.
Este o clasa abstracta "Prototype", care contine o singura metoda "Clone", care va fi folosita pentru clonarea obiectului de tip "Car".
In clasa "Car" este implementarea metodei Clone, in care se returneaza un nou obiect de tip "Car".

Este o clasa abstracta "CarBuilder" care seteaza parametrii pentru un obiect de tip "Car", si metoda GetResult care va fi folosita pentru a obtine instanta dorita de Car.
Avem clasa FordBuilder si HennsseyBuilder care reprezina masini concrete cu parametri concreti.
Clasa "Director" este acel care "ordoneaza" builder-ului sa construiasca masina dorita.

In clasa Main, facem instanta la un director si la 2 builderi(pentru fiecare tip de Builder care avem). Directorul ordoneaza unui builder sa construiasca o masina, iar apoi o instanta de tip Car preia rezultatul obtinut in urma crearii masinii de catre builder.
Obiectul car3 este obtinut in urma clonarii obiectului car1. Din cauza ca Clone() returneaza obiect de tip Prototype, trebuie sa-i facem casting in tip Car.
