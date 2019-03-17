namespace BrawlLib.SSBB
{
    public class Fighter
    {
        /// <summary>
        /// The character slot index, as used by common2.pac event match and all-star data.
        /// See: http://opensa.dantarion.com/wiki/Character_Slots
        /// </summary>
        public int ID { get; private set; }
        /// <summary>
        /// The fighter name (e.g. "Yoshi").
        /// </summary>
        public string Name { get; private set; }

        public Fighter(int id, string name)
        {
            this.ID = id;
            this.Name = name;
        }

        public override string ToString() { return Name; }

        public readonly static Fighter[] Fighters = new Fighter[] {
            //          ID     Display Name     
			new Fighter(0x00, "Mario"),
            new Fighter(0x01, "Donkey Kong"),
            new Fighter(0x02, "Link"),
            new Fighter(0x03, "Samus"),
            new Fighter(0x04, "Zero Suit Samus"),
            new Fighter(0x05, "Yoshi"),
            new Fighter(0x06, "Kirby"),
            new Fighter(0x07, "Fox"),
            new Fighter(0x08, "Pikachu"),
            new Fighter(0x09, "Luigi"),
            new Fighter(0x0a, "Captain Falcon"),
            new Fighter(0x0b, "Ness"),
            new Fighter(0x0c, "Bowser"),
            new Fighter(0x0d, "Peach"),
            new Fighter(0x0e, "Zelda"),
            new Fighter(0x0f, "Sheik"),
            new Fighter(0x10, "Ice Climbers"),
            new Fighter(0x11, "Popo"),
            new Fighter(0x12, "Nana"),
            new Fighter(0x13, "Marth"),
            new Fighter(0x14, "Mr. Game & Watch"),
            new Fighter(0x15, "Falco"),
            new Fighter(0x16, "Ganondorf"),
            new Fighter(0x17, "Wario"),
            new Fighter(0x18, "Meta Knight"),
            new Fighter(0x19, "Pit"),
            new Fighter(0x1a, "Pikmin & Olimar"),
            new Fighter(0x1b, "Lucas"),
            new Fighter(0x1c, "Diddy Kong"),
            new Fighter(0x1d, "Charizard"),
            new Fighter(0x1e, "Charizard (independent)"),
            new Fighter(0x1f, "Ivysaur"),
            new Fighter(0x20, "Ivysaur (independent)"),
            new Fighter(0x21, "Squirtle"),
            new Fighter(0x22, "Squirtle (independent)"),
            new Fighter(0x23, "King Dedede"),
            new Fighter(0x24, "Lucario"),
            new Fighter(0x25, "Ike"),
            new Fighter(0x26, "R.O.B."),
            new Fighter(0x27, "Jigglypuff"),
            new Fighter(0x28, "Toon Link"),
            new Fighter(0x29, "Wolf"),
            new Fighter(0x2a, "Snake"),
            new Fighter(0x2b, "Sonic"),
            new Fighter(0x2c, "Giga Bowser"),
            new Fighter(0x2d, "Warioman"),
            new Fighter(0x2e, "Red Alloy (don't use in event matches)"),
            new Fighter(0x2f, "Blue Alloy (don't use in event matches)"),
            new Fighter(0x30, "Yellow Alloy (don't use in event matches)"),
            new Fighter(0x31, "Green Alloy (don't use in event matches)"),
            new Fighter(0x32, "Roy (PM)"),
            new Fighter(0x33, "Mewtwo (PM)"),
            new Fighter(0x3F, "EXCharacter 3F"),
            new Fighter(0x40, "EXCharacter 40"),
            new Fighter(0x41, "EXCharacter 41"),
            new Fighter(0x42, "EXCharacter 42"),
            new Fighter(0x43, "EXCharacter 43"),
            new Fighter(0x44, "EXCharacter 44"),
            new Fighter(0x45, "EXCharacter 45"),
            new Fighter(0x46, "EXCharacter 46"),
            new Fighter(0x47, "EXCharacter 47"),
            new Fighter(0x4B, "EXCharacter 4B"),
            new Fighter(0x4C, "EXCharacter 4C"),
            new Fighter(0x4D, "EXCharacter 4D"),
            new Fighter(0x4E, "EXCharacter 4E"),
            new Fighter(0x4F, "EXCharacter 4F"),
            new Fighter(0x50, "EXCharacter 50"),
            new Fighter(0x51, "EXCharacter 51"),
            new Fighter(0x52, "EXCharacter 52"),
            new Fighter(0x53, "EXCharacter 53"),
            new Fighter(0x54, "EXCharacter 54"),
            new Fighter(0x55, "EXCharacter 55"),
            new Fighter(0x56, "EXCharacter 56"),
            new Fighter(0x57, "EXCharacter 57"),
            new Fighter(0x58, "EXCharacter 58"),
            new Fighter(0x59, "EXCharacter 59"),
            new Fighter(0x5A, "EXCharacter 5A"),
            new Fighter(0x5B, "EXCharacter 5B"),
            new Fighter(0x5C, "EXCharacter 5C"),
            new Fighter(0x5D, "EXCharacter 5D"),
            new Fighter(0x5E, "EXCharacter 5E"),
            new Fighter(0x5F, "EXCharacter 5F"),
            new Fighter(0x60, "EXCharacter 60"),
            new Fighter(0x61, "EXCharacter 61"),
            new Fighter(0x62, "EXCharacter 62"),
            new Fighter(0x63, "EXCharacter 63"),
            new Fighter(0x64, "EXCharacter 64"),
            new Fighter(0x65, "EXCharacter 65"),
            new Fighter(0x66, "EXCharacter 66"),
            new Fighter(0x67, "EXCharacter 67"),
            new Fighter(0x68, "EXCharacter 68"),
            new Fighter(0x69, "EXCharacter 69"),
            new Fighter(0x6A, "EXCharacter 6A"),
            new Fighter(0x6B, "EXCharacter 6B"),
            new Fighter(0x6C, "EXCharacter 6C"),
            new Fighter(0x6D, "EXCharacter 6D"),
            new Fighter(0x6E, "EXCharacter 6E"),
            new Fighter(0x6F, "EXCharacter 6F"),
            new Fighter(0x70, "EXCharacter 70"),
            new Fighter(0x71, "EXCharacter 71"),
            new Fighter(0x72, "EXCharacter 72"),
            new Fighter(0x73, "EXCharacter 73"),
            new Fighter(0x74, "EXCharacter 74"),
            new Fighter(0x75, "EXCharacter 75"),
            new Fighter(0x76, "EXCharacter 76"),
            new Fighter(0x77, "EXCharacter 77"),
            new Fighter(0x78, "EXCharacter 78"),
            new Fighter(0x79, "EXCharacter 79"),
            new Fighter(0x7A, "EXCharacter 7A"),
            new Fighter(0x7B, "EXCharacter 7B"),
            new Fighter(0x7C, "EXCharacter 7C"),
            new Fighter(0x7D, "EXCharacter 7D"),
            new Fighter(0x7E, "EXCharacter 7E"),
            new Fighter(0x7F, "EXCharacter 7F"),

            // Event Matches
            new Fighter(0x3e, "None / Select Character"),
            new Fighter(0x48, "Pokémon Trainer"),
            new Fighter(0x49, "Samus/ZSS"),
            new Fighter(0x4a, "Zelda/Sheik")
        };
    }
}
