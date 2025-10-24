using System;
using UnityEngine;

namespace Lucecita.HappinessBlossom.Common
{
    public class UpdateExecutor : MonoBehaviour
    {
        public static Action onUpdate = delegate { };
        
        private void Update()
        {
            onUpdate();
        }
    }
   
}