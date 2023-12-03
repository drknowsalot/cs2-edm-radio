
using System.IO;
using static Game.Audio.Radio.Radio;

namespace MyRadioMod 
{
	public class MyRadioMod 
	{		
		private static string radioDirec = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "CustomRadio");
		public static void LoadMyRadio() {
			RadioNetwork radioNetwork = ExtendedRadio.ExtendedRadio.JsonToRadioNetwork(radioDirec+"\\TestNetwork");
			RadioChannel radioChannel = ExtendedRadio.ExtendedRadio.JsonToRadio(radioDirec+"\\TestNetwork\\JsonAndPath", radioNetwork.name);

			ExtendedRadio.ExtendedRadio.AddRadioNetworkToTheGame(radioNetwork);
			ExtendedRadio.ExtendedRadio.AddRadioChannelToTheGame(radioChannel);
		}
	}
}