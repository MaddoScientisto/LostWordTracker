namespace LostWordTracker.Data
{
    public class CharacterStorage
    {
        public int Id { get; set; }        
        public bool Obtained { get; set; }
        public int Level { get; set; }
        public int Skill1 { get; set; }
        public int Skill2 { get; set; }
        public int Skill3 { get; set; }
        public int Rank { get; set; }
        public int LimitBreak { get; set; }
        public int Awakening { get; set; }

        public bool Rebirth { get; set; }

        public CharacterStorage()
        {
            Id = 0;
            Obtained = false;
            Level = 0;
            LimitBreak = 0;
            Awakening = 0;
            Skill1 = 0;
            Skill2 = 0;
            Skill3 = 0;
            Rank = 0;
            Rebirth = false;
        }

        public CharacterStorage(int id)
        {
            Id = id;
            Obtained = false;
            Level = 0;
            LimitBreak = 0;
            Awakening = 0;
            Skill1 = 0;
            Skill2 = 0;
            Skill3 = 0;
            Rank = 0;
            Rebirth = false;
        }
        
    }
}
