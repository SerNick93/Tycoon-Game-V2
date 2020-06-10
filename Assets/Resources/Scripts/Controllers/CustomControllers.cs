using UnityEngine;

namespace Nick.controllers
{
    public class CustomControllers
    {
        //Find the mouse position on a plane when in a perspective camera mode.
        public static Vector3 GetWorldPositionOnPlane(Vector3 screenPosition, float y)
        {
            Ray ray = Camera.main.ScreenPointToRay(screenPosition);
            Plane xy = new Plane(Vector3.up, new Vector3(0, y, 0));
            float distance;
            xy.Raycast(ray, out distance);
            return ray.GetPoint(distance);
        }

        //#region Used for model placement and editing
        ////Do not snap the object to the grid.
        public static Vector3 NonSnapToGrid(Vector3 noGridSnap)
        {
            noGridSnap = new Vector3(
                            GameManager.MyInstance.Grid.WorldToLocal(GameManager.MyInstance.MousePoisition).x,
                            0,
                            GameManager.MyInstance.Grid.WorldToLocal(GameManager.MyInstance.MousePoisition).y
                            );

            return noGridSnap;
        }

        ////Do snap the object to the grid.
        public static Vector3 SnapToGrid(Vector3 gridSnap)
        {
            gridSnap = new Vector3(
                        GameManager.MyInstance.Grid.WorldToCell
                        (GameManager.MyInstance.MousePoisition).x,
                        0,
                        GameManager.MyInstance.Grid.WorldToCell
                        (GameManager.MyInstance.MousePoisition).y
                        );



            return gridSnap;
        }
        //#endregion
        public static BoundsInt GameObjectBounds(Transform go, BoxCollider cc)
        {
            Transform child = go.transform.GetChild(0).GetComponentInChildren<Transform>();

            BoundsInt area = new BoundsInt(
                (Mathf.RoundToInt(cc.bounds.center.x - cc.bounds.extents.x)),
                (Mathf.RoundToInt(cc.bounds.center.y - cc.bounds.extents.y)),
                (Mathf.RoundToInt(cc.bounds.center.z - cc.bounds.extents.z)),
                (Mathf.RoundToInt(cc.bounds.size.x)),
                (1),
                (Mathf.RoundToInt(cc.bounds.size.z)));
            Debug.Log(area);

            return area;
            //BoundsInt f = new BoundsInt()
        }
        public static Vector3Int GameObjectPosition(BoxCollider cc)
        {
            Vector3Int area = new Vector3Int(
                (Mathf.RoundToInt(cc.bounds.max.x)),
                0,
                (Mathf.RoundToInt(cc.bounds.max.z))
                );
            Debug.Log(area);
            return area;
        }

        public static void DestroyGameObject(GameObject go)
        {
                Object.Destroy(go);
                GameManager.ActiveModels.Remove(go);
        }
        public static void DestroyMultipleGameObjects(GameObject[] GOs)
        {
            for (int i = 0; i < GOs.Length; i++)
            {
                Object.Destroy(GOs[i]);
                GameManager.ActiveModels.Remove(GOs[i]);
            }
        }
    }
}
