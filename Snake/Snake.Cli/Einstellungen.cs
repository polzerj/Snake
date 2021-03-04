using System;

namespace Snake.Cli
{
    [Serializable]
    public class Einstellungen
    {
//        <?xml version = "1.0" ?>
//< Einstellungen xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
//  <OldSmiley>true</OldSmiley>
//  <Sound>true</Sound>
//  <Autorun>true</Autorun>
//  <Spielfeldgroeße>20</Spielfeldgroeße>
//</Einstellungen>
        public bool OldSmiley { get; set; }
        public bool Sound   { get; set; }
        public bool Autorun { get; set; }
        public int Spielfeldgroeße { get; set; }
        public Einstellungen()
        {

        }
    }
}
