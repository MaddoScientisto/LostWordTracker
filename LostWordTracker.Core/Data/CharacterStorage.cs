namespace LostWordTracker.Data
{
    public class CharacterStorage
    {
        public int Id { get; set; }        
        public bool Obtained { get; set; }
        public int Level { get; set; }
        public int LimitBreak { get; set; }
        public int Awakening { get; set; }

        public CharacterStorage()
        {
            Id = 0;
            Obtained = false;
            Level = 0;
            LimitBreak = 0;
            Awakening = 0;
        }

        public CharacterStorage(int id)
        {
            Id = id;
            Obtained = false;
            Level = 0;
            LimitBreak = 0;
            Awakening = 0;
        }
        
    }
}
