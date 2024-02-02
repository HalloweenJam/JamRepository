using UnityEngine;

namespace Core
{
    public static class ComponentExtensions
    {
        public static void Activate(this Component component)
        {
            if (!component.gameObject.activeSelf && component.gameObject != null)
                component.gameObject.SetActive(true);
        }

        public static void Deactivate(this Component component) 
        {
            if(component.gameObject.activeSelf && component.gameObject != null)
                component.gameObject.SetActive(false);
        }
    }
}
