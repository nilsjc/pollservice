<h1>Välkommen till gallupverktyget.</h1><br>
I skrivande stund är det inte färdigt, men det finns en fungerande backend och api för att:
- skapa undersökningar.
- rösta.
- samla in totalt antal röster.<br><br>

När mar röstar under ett unikt namn så skapas en användare. Just nu finns det ingen autentisering eller liknande. Och en användare kan rösta om obegränsat antal gånger. Däremot viss validering av data.<br>

Backend:en arbetar med en databas. Den består av följande tabeller:<br>
- tbl_poll
- tbl_question
- tbl_user
- tbl_answer

Databasens, som för övrigt lokalt körs med sqlite, namn är hårdkodad i program.cs. Den bör naturligtvis flyttas över till en appsettingsfil. Men om databasen inte existerar så skapas en ny.

Följande är TODOs
- klient/frontend
- enhetstester
- pipeline för publicering, exempelvis mot Azure
- fylla på med stor mängd data och skapa prestandamätningar
- skruva åt säkerheten, exempelvis möjligheten att inte rösta om flertal gånger
