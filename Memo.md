##Torkil & August Memo

 * (1) Kan man erstatte biter av fysikkmotoren i unity3d med egen fysikk / ligninger / metoder / funksjoner?

Unity har innebygde funksoner for spillfysikk, dette inkluderer simulering av kollisjoner, krefter og mer. Fysikken er laget for at den enkelt skal kunne brukes av utviklere uten mye kunnskap om fysikk. med den innebygde fysikken kan en legge fart til et objekt, og unity vil simulere alt som trengs an posisjon, krefter, friksjon, etc.
Unity fysikken er koblet opp til en komponent av et objekt kalt rigidbody, denne komponenten er ikke nødvendig å aktivere og uten vil ikke objekter i unity ha noe innebygd fysikk. Man har direkte tilgang til posisjonen til objekter i unity, med dette kan man lage egen system som håndterer bevegelse. Så kan man legge til egne vektorer for fart, akselerasjon, krefter, masse, etc for å syre fysikk.
Selv med RigidBody satt på et objekt kan man sette fysikken av og unngå å bruke ferdigmetodene for å simulere fysikken selv.


 * (2) Kan dere putte inn egne ligninger for bevegelse av et objekt inn i et spill? Scripte bevegelse, med egenkomponert kode som bestemmer bevegelsen?

Ja man kan gjøre dette. Men siden Unity er bygd veldig rundt den innebygde fysikken så kan den litt tungvindt av og til. På nettet blir man oppmuntret til å bruke
den innebygde fysikkmotoren, men det blir også sagt at det er fullt mulig å lage sin egen. Bevegelse kan man lage med hjelp av vectorer, koordinatsystemer og funksjoner for disse.
 

 * (3) Kan dere gi som input, ei fil med koordinater som beskriver en bevegelse, og så la unity bevege en gjenstand langs disse koordinatene?

Dette kan gjøres ved å lese fra f.eks. en CVS eller XML fil, hvor man kan bruke en CVS reader class og legge koordinat dataene inn i et array og derfra manipulere disse koordinatene til å bevege en gjenstand.


 * (4) Kan dere bruke unity i dette kurset, eller er den helt satt med den eksisterende ferdige spillmotoren?

Som forklart i tidligere spørsmål så kan man bruke Unity i dette kurset og utvikle en egen fysikkmotor. Det unity kan gi tilbake er en veldig enkel å rask måte å vise grafikk på, dette gjør at vi kan fokusere mer på faget og likevel få en god visuell representasjon av arbeidet.


 * (5) Hvilke andre alternativer er det til programmer/språk til å bruke i dette kurset?

Man kan bruke andre alternativer som f.eks. Java, OpenGL, C++ etc. Men grafikken må man da eventuelt lage selv, eller blir isåfall ikke like lett å få fram. Vi har også brukt mye tid på unity i spillprogrammering i forrige semester, så vi vil kunne fort starte med unity og C# scripting igjen.


 * (6) Hva mener dere er det beste verktøyet å bruke i dette kurset?

Vi mener det beste verktøyet å bruke i dette faget er Unity ettersom man kan på enklest måte få fram grafikk, noe som er veldig kjekt for å se hva man koder ser ut som, om det oppfører seg som det skal.

 
 