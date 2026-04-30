
using BLINK.RPGBuilder.Combat;
using UnityEngine;
using BLINK.RPGBuilder.Characters;
using BLINK.Controller;
using UnityEngine.SceneManagement;


namespace BLINK.RPGBuilder.Managers
{
    public class CustomTargetMarker : MonoBehaviour
    {
        [SerializeField] public GameObject EnemyTargetIcon;
        [SerializeField] public GameObject FriendlyTargetIcon;
        [SerializeField] public GameObject SelectedTargetIcon;
        public Camera cam;
        public GameObject reticle;          // your quad or world-space UI root
        public LayerMask groundMask;       // set to Terrain (and any walkable ground)
        [Header("Tuning")]
        public float maxRayDistance = 2000f;
        public float heightOffset = 0.01f; // prevents z-fighting
        public float followSmoothing = 20f; // 0 = snap, higher = smoother
        public CombatEntity CurrentTarget;
        public string sceneNameToCheck = "Main-Menu";
        TopDownClickToMoveController playerController;
        void Reset()
        {
            cam = Camera.main;
        }
        public void Start()
        {
            CurrentTarget = null;
        }

        public void LateUpdate()
        {
            if(IsSceneLoaded(sceneNameToCheck))
            {
                
            }
            else
            {
                if (CurrentTarget != null && !CurrentTarget.IsPlayer())
                {
                    foreach (var marker in CurrentTarget.GetComponentsInChildren<CustomTargetMarker>())
                        marker.transform.forward = Camera.main.transform.forward;
                }
                if (CurrentTarget != null && !CurrentTarget.IsPlayer() && Input.GetMouseButtonDown(0) && CurrentTarget.tag.Contains("enemy") && !CurrentTarget.IsDead())
                {
                    if (Vector3.Distance(CurrentTarget.transform.position, GameState.playerEntity.transform.position) <= 3)
                    {
                        ActionBarSlot Slot = ActionBarManager.Instance.actionBarSlots[0];
                        if(Slot.ThisAbility !=  null)
                        {
                            playerController = GameState.playerEntity.gameObject.GetComponent<TopDownClickToMoveController>();
                            playerController.LookAtCursor();
                            Debug.Log(Slot.ThisAbility.name);
                            Slot.ClickUseSlot();
                        }
                    }
                }
                else if (CurrentTarget != null && !CurrentTarget.IsPlayer() && Input.GetMouseButtonDown(1) && CurrentTarget.tag.Contains("enemy") && !CurrentTarget.IsDead())
                {
                    playerController = GameState.playerEntity.gameObject.GetComponent<TopDownClickToMoveController>();
                    ActionBarSlot Slot = ActionBarManager.Instance.actionBarSlots[1];
                    playerController.LookAtCursor();
                    Slot.ClickUseSlot();
                }
            }        
        }

        bool IsSceneLoaded(string sceneName)
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);
                if (scene.name == sceneName && scene.isLoaded)
                {
                    return true;
                }
            }
            return false;
        }
        private void OnEnable()
        {
            CurrentTarget = null;
            CombatEvents.TargetChanged += TargetChanged;
        }

        private void OnDisable()
        {
            CombatEvents.TargetChanged -= TargetChanged;
        }

        protected virtual void DestroyCrosshair()
        {
            DestroyAll("target-crossair");
            GameState.playerEntity.ResetTargetNameplate();
        }
        protected virtual void TargetChanged(CombatEntity entity, CombatEntity newTarget)
        {
           TopDownClickToMoveController Controller = GameObject.FindGameObjectWithTag("Player").GetComponent<TopDownClickToMoveController>();
           Controller.m_groundClick.AddListener(DestroyCrosshair);
            InitTargetUI(newTarget);
            CurrentTarget = newTarget;
        }


        public void DestroyAll(string tag)
        {
            GameObject[] targets = GameObject.FindGameObjectsWithTag(tag);
            for (int i = 0; i < targets.Length; i++)
            {
                Destroy(targets[i]);
            }
            
        }

        protected virtual void InitTargetUI(CombatEntity newTarget)
        {
            if (newTarget != null)
            {
                CurrentTarget = newTarget;
                DestroyAll("target-crossair");
                if (newTarget.gameObject.tag == "enemy-npc")
                {
                    ReticleHandler reticleHandler = null;
                    GameObject obj = Instantiate(EnemyTargetIcon, CurrentTarget.transform);
                    reticleHandler = obj.GetComponent<ReticleHandler>();
                    reticleHandler.target = CurrentTarget.transform; 
                }
                else if (newTarget.gameObject.tag.Contains("friendly"))
                {
                    GameObject obj = Instantiate(FriendlyTargetIcon, CurrentTarget.transform);
                    ReticleHandler reticleHandler = null;
                    reticleHandler = obj.GetComponent<ReticleHandler>();
                    reticleHandler.target = CurrentTarget.transform;
                }
            }
        }
    }
}