# MapProcGen
------------

- Map ziet er al goed uit qua layout
- Onderverdeling in 10x10 lijkt ook wel coole resultaten te geven

## Todo (of mss meer ToThinkAbout)
   -------------------------------

- zijn alle tiles in een 10x10 verbonden met elkaar
    - uitdeinende rechthoek om dan hetzelfde te doen als bij connecties
    - checken met pathfinding?
    - nieuwe map genereren (VERMIJD DIT, DAT ZIJN WSS DE INTERESSANTSTE)

- connecties
    - alle edges afgaan (per 10x10)
        - naburige edges linken (zeker als ze een 1 op 1 paar vormen)
    - lijnen trekken tussen naburige 10x10 (kan een fallback zijn)
        - je kan daar ook de meest naburige verbinden

- kamerbewustzijn
    - de twee vorige processen bepalen ook wat de afzonderlijke ruimtes zijn zodat je ze als geheel kan aansreken
        (dit is nodig om de kamers te tonen zoals we dat nu doen)