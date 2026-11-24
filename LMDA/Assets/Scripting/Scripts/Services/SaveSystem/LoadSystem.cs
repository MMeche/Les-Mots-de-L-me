using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Linq;

public static class LoadSystem
{
    public static SaveData LoadGameData(string fileName)
    {
        try
        {
            string fileContent = File.ReadAllText(fileName);
            SaveData saveData = JsonUtility.FromJson<SaveData>(fileContent);
            List<PlayerStatus> playerStatuses = Resources.FindObjectsOfTypeAll<PlayerStatus>().ToList();
            for (int i = 0; i < playerStatuses.Count; i++)
            {
                PlayerSaveData correctEntry = saveData._allPlayerSaveData.Find(ps => ps.CharName == playerStatuses[i].CharName);

                playerStatuses[i].PlayerIndex = correctEntry.PlayerIndex;
                playerStatuses[i].CharName = correctEntry.CharName;
                playerStatuses[i].Position = correctEntry.Position;
                playerStatuses[i].Level = correctEntry.Level;
                playerStatuses[i].Experience = correctEntry.Experience;
                playerStatuses[i].MaxHealth = correctEntry.MaxHealth;
                playerStatuses[i].Health = correctEntry.Health;
                playerStatuses[i].AvailableSkillPoints = correctEntry.AvailableSkillPoint;
                playerStatuses[i].EnemiesDefeated = correctEntry.EnemiesDefeated;
                playerStatuses[i].Stats[StatType.Poetic] = correctEntry.PoeticStats;
                playerStatuses[i].Stats[StatType.Narrative] = correctEntry.NarrativeStats;
                playerStatuses[i].Stats[StatType.Lyrical] = correctEntry.LyricStats;
                playerStatuses[i].Stats[StatType.Argumentative] = correctEntry.ArgStats;
            }
            Resources.FindObjectsOfTypeAll<TutorialState>()[0].Assign(saveData._tutorialSaveData);
            Resources.FindObjectsOfTypeAll<InteractedSteleData>()[0].Assign(saveData._journalSteleSaveData);
            return saveData;
        }
        catch
        {
            return null;
        }
    }
}
