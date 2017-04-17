using UnityEngine;

public static class Service {
	public static GameObject player{get;set;}

	static private Color color_1 = new Color32(248,58,146,255);
	static private Color color_2 = new Color32(123,197,116,255);
	static private Color color_3 = new Color32(255,171,92,255);
	static private Color color_4 = new Color32(38,47,38,255);

	static public Color[] ColorLibrary = new Color[4]{color_1,color_2,color_3,color_4};
	static public SceneManager<TransitionData> sceneManager;
	static public EventManager eventManager;
	static public EnemyManager enemyManager;
	static public PrefebObj prefebObj;
}
