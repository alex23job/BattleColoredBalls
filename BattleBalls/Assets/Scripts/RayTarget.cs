using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayTarget : MonoBehaviour
{
    [SerializeField] private LevelControl level;

    private Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //Vector3 point = new Vector3(cam.pixelWidth / 2, cam.pixelHeight / 2, 0);
            //Vector3 point = cam.ScreenToWorldPoint(Input.mousePosition);
            //Ray ray = cam.ScreenPointToRay(point);
            //RaycastHit hit;
            //if (Physics.Raycast(ray, out hit))
            //{
            //    Debug.Log("Hit " + hit.point);
            //    StartCoroutine(SphereIndicator(hit.point));
            //}
            RaycastHit hit;
            Ray MyRay;
            MyRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(MyRay.origin, MyRay.direction * 10, Color.yellow);
            if (Physics.Raycast(MyRay, out hit, 100))
            {
                //MeshFilter filter = hit.collider.GetComponent(typeof(MeshFilter)) as MeshFilter;
                //Debug.Log($"name={hit.collider.gameObject.name} tag={hit.collider.gameObject.tag}");
                if (hit.collider.gameObject.CompareTag("tiletail"))
                {
                    if (level != null)
                    {
                        if (level.isClick) level.TranslatePosition(hit.point);
                    }
                    //StartCoroutine(SphereIndicator(hit.point));
                    return;
                }
                /*if (hit.collider.gameObject.CompareTag("tile"))
                {
                    if (level != null)
                    {
                        level.SelectTile(hit.collider.gameObject);
                    }
                    //StartCoroutine(SphereIndicator(hit.point));
                }*/
                /*if (hit.collider.gameObject.CompareTag("target"))
                {
                    if (level != null)
                    {
                        Vector3 pos = hit.collider.transform.position;
                        if (pos.z < -12f) pos.y += 0.3f;
                        if (pos.x > 4f) pos.y += 1.9f;
                        level.TileMove(pos);
                    }
                }*/

                //if (filter)
                //{
                //    //имя обьекта по которому щелкнули мышей               
                //    Debug.Log(filter.gameObject.name);
                //    Debug.Log(filter.gameObject.tag);
                //    //Debug.Log("Hit " + hit.point);
                //    if (filter.gameObject.CompareTag("tiletail"))
                //    {
                //        if (level != null)
                //        {
                //            level.TranslatePosition(hit.point);
                //        }
                //        StartCoroutine(SphereIndicator(hit.point));
                //    }
                //}
            }
        }
    }

    private IEnumerator SphereIndicator(Vector3 pos)
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = pos;
        sphere.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        yield return new WaitForSeconds(1);
        Destroy(sphere);
    }
}
