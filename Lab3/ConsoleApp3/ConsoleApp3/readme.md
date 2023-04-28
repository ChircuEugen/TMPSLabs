Avem clasa "Employee" cu anumiti parametri: id, nume, salariu, rol. La fel avem un ThirdParty calculator care calculeaza salariul total, problema este ca acel ThirdParty
calculator, ca argument, primeste un array de int-uri, care le sumeaza si returneaza suma totala.
Avem interfata "ITarget" in care este metoda AdaptProcessSalaries care primeste ca argument o lista de Employee.
Avem clasa "EmployeeAdapter" care mosteneste de la interfata ITarget. EmployeeAdapter implementeaza metoda din ITarget, ProcessSalaries. Adapterul trebuia sa faca posibil
ca sa putem trimete ca argument o lista de Employee, care trebuie sa execute metoda ProcessSalaries, care primeste ca argument un array de int-uri.
Adapterul preia valorile sariale ale fiecurau Employee, le adauga intr-un array de int-uri, care la urma lui va fi folosit ca argument pentru functia ProcessSalaries.

Avem interfata "ISharedFolder" care are o metoda ProcessOperation care ca argument primeste un Employee si o lista de Employee.
Clasa "SharedFolder" implenteaza metoda interfetei,argumentul careia reprezinta lista asupra caror lucratori se fac modificarile.
Clasa SharedFolderProxy este folosita ca un protection proxy si verifca rolul persoanei care doreste sa faca schimbari. Daca rolul este Developer - proxy-ul respinge 
cererea de a face schimbarea, in caz contrar permite.

In clasa Main, este creata o lista de Employee. Obiectul target, de tip ITarget este folosit pentru a calcula salariile lucratorilor.
Obiectul sharedFolder1 primeste ca argument in constructor un lucrator cu rolul de Director, iar sharedFolder primeste ca argument un lucrator cu rolul de Developer.
In cazul directorului, proxy-ul ii garanteaza acces, in cazul developerul - il respinge.
