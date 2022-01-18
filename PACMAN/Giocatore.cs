namespace PACMAN
{
    class Giocatore
    {
        public string Nome { get; set; }
        public int Record { get; set; }
        public int Livello { get; set; }
        public Giocatore() { }
        public Giocatore(string[] vals)
        {
            Nome = vals[0];
            Record = int.Parse(vals[1]);
        }
        public override string ToString()
        {
            return $"{Nome};{Record};{Livello}";
        }
    }
}
