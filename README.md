# RestaurantAB
Labb 1: ASP.NET API + Databas

Om uppgiften
I denna uppgift ska du utveckla ett komplett backend‑system som hanterar bokningar, kundinformation, bord och meny för en valfri verksamhet inom mat och dryck, till exempel en restaurang, ett kafé eller ett bageri. Systemet ska exponeras som ett REST API och vara redo att användas av en framtida webbaserad frontend. Målet är att leverera ett färdigt backend‑system som kan administrera restaurangens resurser och hantera bokningsförfrågningar.
Vad du ska göra
Applikationen/databasen
Först ska du skapa en databas med code-first modellen och definiera strukturen för att hantera de centrala delarna av systemet. Databasen ska kunna hantera följande:
Systemet ska kunna lagra information om restaurangens bord, inklusive kapacitet och bordsnummer.
Systemet ska även hantera administratörer med uppgifter för inloggning som används av API:et för JWT‑autentisering. Förslagsvis Username och PasswordHash
Systemet ska kunna lagra grundläggande kontaktuppgifter för den person som gör bokningen (t.ex namn och telefonnummer).
Det ska gå att lagra bokningar som kopplas till specifika bord och till den person som bokat, och som innehåller information om datum, tid och antal gäster.
En bokning varar som standard i två timmar, vilket innebär att ett bord ska anses upptaget under den perioden.
Systemet ska även hantera en meny där varje rätt har ett namn, pris, beskrivning 
samt en kolumn IsPopular (bool) som markerar om rätten är populär.
EXTRA: Lägg till en kolumn BildUrl av typen string som ska innehålla en webbadress (URL) till en bild på rätten. Det räcker att spara länken som text; själva bilden lagras inte i databasen.
Skapa ett REST-API
Nästa steg är att utveckla ett REST-API med ASP.NET Core som ska stödja följande funktioner:
Hantera restaurangens bord och kunder med möjlighet att lägga till, uppdatera, läsa och ta bort information.
Skapa och hantera bokningar, där ett bord endast kan bokas om det är ledigt vid vald tidpunkt.
En bokning gör bordet upptaget i två timmar från starttid, vilket används vid kontroll av tillgänglighet.
Hämta en lista över lediga bord för ett valt datum, klockslag och antal gäster.
Endast bord som inte har någon bokning som överlappar den valda tiden och som har tillräcklig kapacitet ska returneras.
Notera: Vid kontroll av lediga bord ska bokningar anses blockera bordet i två timmar från sin starttid.
Exempel: Om ett bord är bokat 18:00 så blockeras det två timmar före och två timmar efter starttiden. Det innebär att bokningar som startar mellan 16:01 och 19:59 inte är tillåtna. Bordet blir ledigt igen 20:00.
Hantera restaurangens meny med möjlighet att administrera rätter (lägga till, uppdatera och ta bort).
Autentisering för administratörer
 API:et ska stödja inloggning för administratörer.
En administratör ska kunna logga in genom att skicka in användarnamn och lösenord till en endpoint (t.ex. POST /login).
Om inloggningen lyckas ska en JWT returneras som används för att komma åt de skyddade endpointsen.
Endast administratörer med en giltig JWT ska kunna hantera bokningar, bord och menyer.
