using UnityEngine;
using System.Collections.Generic;
using BLINK.RPGBuilder.Characters;
using BLINK.RPGBuilder.Data;
using BLINK.RPGBuilder.Managers;
using BLINK.RPGBuilder.Templates;
using BLINK.RPGBuilder.UIElements;
using BLINK.RPGBuilder.WorldPersistence;
namespace BLINK.RPGBuilder.World
{
    public class DestroyInteractComponent : MonoBehaviour
    {
        public void DestroyComponent()
        {
            Destroy(GetComponent<InteractableObject>());
            Destroy(GetComponent<MeshCollider>());
        }
    }
}
