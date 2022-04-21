namespace LostWordTracker.Data
{
    public class CharacterDefinition
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Universe { get; set; }
        public string Portrait { get; set; }
        public string Tier { get; set; }
        public int TierRank { get; set; }
        public string FarmTier { get; set; }
        public string CqTier { get; set; }

        public string ObtainType { get; set; }

        public bool Enabled { get; set; }

        public bool HasRebirth { get; set; }
        public string RebirthPortrait { get; set; }
    }
}
