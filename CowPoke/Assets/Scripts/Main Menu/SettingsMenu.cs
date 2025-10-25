using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
   public GameObject settingsMenu;
   public GameObject mainMenu;
   public void SetVolume(float volume)
   {
      Debug.Log(volume);
   }
   public void BackToMainMenu()
   {
      mainMenu.SetActive(true);
      settingsMenu.SetActive(false);
   }

   public void OpenSettings()
   {
      mainMenu.SetActive(false);
      settingsMenu.SetActive(true);
   }
}
