public static class HeroCostHelper
{
    public static int GetCost(HeroGrade grade)
    {
        return grade switch
        {
            HeroGrade.Normal => 5,
            HeroGrade.Rare => 10,
            HeroGrade.Epic => 15,
            HeroGrade.Legendary => 20,
            _ => 0
        };
    }
}