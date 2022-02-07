### To be discussed
Locaties
Naam
Adres

## Graphical Overview
::: mermaid
graph TB;
Student --> Course;
Employee --> Course;
Group --> Student
:::


## Defined attributes

| UI             | Attribute   | DomainModel | Type     |
|----------------|-------------|-------------|----------| 
| Voornaam       | FirstName   | Student     | string   | 
| Achternaam     | LastName    | Student     | string   |
| Adres          | Address     | Student     | string   |
| Huisnummer     | HouseNumber | Student     | int      |
| Postcode       | PostalCode  | Student     | string   |
| Plaats         | City        | Student     | string   |
| Telefoonnummer | Telephone   | Student     | string   |
| Mobielnummer   | Mobile      | Student     | string   |
| Geboortedatum  | BirthDate   | Student     | datetime |
| E-mail         | Email       | Student     | string   |
| Startdatum     | StartDate   | Student     | datetime |
|
| Naam           | Name        | Course      | string   |
| Type(Voltijd - deeltijd)| Type | Course    | int      |
| Tijdsduur      | Duration    | Course      | int      |
| Semesters      | Semesters   | Course      | Semester[]|
|
| Naam           | Name        | Semester    | string   |
| Start          | StartDate   | Semester    | datetime |
| Eind           | EndDate     | Semester    | datetime |
| Cursus         | Course      | Semester    | Course   |
|
| Naam           | Name        | Class       | string   |
| Semester       | Semester    | Class       | Semester |
| Studenten      | Students    | Class       | Student[]|

