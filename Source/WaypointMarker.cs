using KSP;
using UnityEngine;

namespace NavHud
{
    public class WaypointMarker
    {
        private GameObject _object;

        private double _r;

        public WaypointMarker()
        {
            _object = CreateSimplePlane();
            _object.GetComponent<Renderer>().material = new Material(Shader.Find("Particles/Additive"));
        }

        public void LoadTexture()
        {
            if(FinePrint.WaypointManager.navWaypoint != null)
            {
                GameObject navWaypointIndicator = GameObject.Find("NavBall").transform.FindChild("vectorsPivot").FindChild("NavWaypoint").gameObject;
                Material material = navWaypointIndicator.GetComponent<Renderer>().sharedMaterial;
                _object.GetComponent<Renderer>().material.mainTexture = material.mainTexture;
                // The colors of the Particles/Additive shader turn out to be twice as bright.. somehow..?
                // So I'll multiply by scaleColor to compensate.
                Color scaleColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
                _object.GetComponent<Renderer>().material.SetColor("_TintColor", material.color * scaleColor);
            } else {
                Debug.LogWarning("Tried to load texture while navWaypoint is not instantiated.");
            }
        }

        // code from enhancednavball
        private GameObject CreateSimplePlane()
        {
            Mesh mesh = new Mesh();

            GameObject obj = new GameObject();
            MeshFilter meshFilter = obj.AddComponent<MeshFilter>();
            obj.AddComponent<MeshRenderer>();
            obj.layer = 5;

            const float uvize = 1f;

            Vector3 p0 = new Vector3(-1, -1, 0);
            Vector3 p1 = new Vector3( 1, -1, 0);
            Vector3 p2 = new Vector3(-1,  1, 0);
            Vector3 p3 = new Vector3( 1,  1, 0);

            mesh.vertices = new[]
            {
                p0, p1, p2,
                p1, p3, p2
            };

            mesh.triangles = new[]
            {
                0, 1, 2,
                3, 4, 5
            };

            Vector2 uv1 = new Vector2(0, 0);
            Vector2 uv2 = new Vector2(uvize, uvize);
            Vector2 uv3 = new Vector2(0, uvize);
            Vector2 uv4 = new Vector2(uvize, 0);

            mesh.uv = new[]
            {
                uv1, uv4, uv3,
                uv4, uv2, uv3
            };

            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            mesh.Optimize();

            meshFilter.mesh = mesh;

            return obj;
        }

        public void SetValues(Values values)
        {
            _r = values.Distance;
            _object.transform.localScale = values.VectorSize * Vector3.one;
        }

        public void SetParent(Transform parent)
        {
            ParentVector(_object, parent);
        }

        private void ParentVector(GameObject vector, Transform parent)
        {
            vector.transform.parent = parent;
            vector.transform.localPosition = Vector3.zero;
            vector.transform.localEulerAngles = Vector3.zero;
        }

        public void SetPositions(Vector3d waypoint)
        {
            _object.transform.localPosition = _r * waypoint;
        }

        public void SetActive(bool active)
        {
            _object.SetActive(active);
        }
    }
}