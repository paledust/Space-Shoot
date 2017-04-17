using UnityEngine;

[CreateAssetMenu(menuName = "LoadedScene")]
public class PrefebObj : ScriptableObject {
	[SerializeField] private GameObject _Boss;
	public GameObject Boss{get{return _Boss;}}
	[SerializeField] private GameObject _StartZone;
	public GameObject StartZone{get{return _StartZone;}}
	[SerializeField] private GameObject _Player;
	public GameObject player{get{return _Player;}}
	[SerializeField] private GameObject _EnemyManager;
	public GameObject enemyManager{get{return _EnemyManager;}}
	[SerializeField] private GameObject[] _Scenes;
	public GameObject[] Scenes{get{return	_Scenes;}}
	[SerializeField] private AudioClip[] _Audio;
	public AudioClip[] SoundEffect{get {return _Audio;}}
}
