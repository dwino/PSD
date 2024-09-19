- Victor interactie
    zet een gaurd op het stukje ervoor
        die gaurd triggerd een interactie met victor, gebaseerd op de toestand van vroegere interacties
        je gaat automatisch getriggerde interacties moeten maken
    en ergens ook een geheugen bijhouden voor de speler, niet enkel voor gesprekken


- the bidirectional links of travelnods you can do in a dictionary of <xpMapstring, Dictionary<point, xpMapstring>>

- Animations only play, no processkeyboard shizzle

- na >play< een hypothetisch scenario met een animatie (animationobject!)
small animation of first encounter

- messagelog

- bug met leven --> deel opgelost door messagelog
                --> gestaggerede acties

- hit animatie (shake, rolluik, balance?, floating damage numbers)

- Idle gamescreen (niet in "combat")

- flavour text




- animationobject 

- Global balance

interaction defines its own
- entities
- palyer options
- next action
- end condition
- loop
- input-processing
- draw Procedures


X player and other entities are treated diffrently

X always aje balance between black and white (never used up)

target balance fot others
50/50 for player

player
    original max life
    original balance
    level (multiplier)
    x shift Black --> White
    x shift White --> Black
    attack other.life - player.white
    heal player.life + player.black
    actions out of balance diminish life? black/white? by same percentage?