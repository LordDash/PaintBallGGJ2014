using UnityEngine;
using System.Collections;

public class GridCreator : MonoBehaviour {
	
	public GameObject grid_object; // Single grid object

	public enum collisionLayer{ NEUTRAL = 8, RED = 9, BLUE = 10 };

	// We need a start position
	public Vector3 startPos = Vector3.zero;
	public GameObject objectToFill = null;

	private Vector3 object_size = Vector3.zero;
	private int x_elements = 0;
	private int y_elements = 0;
	
	// Use this for initialization
	void Start () {
		createGrid();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void createGrid()
	{
		float x_pos = 0.0f;
		float y_pos = 0.0f;

		// Define the amount of elements we need.
		GameObject grid_first_object = (GameObject)Instantiate( grid_object, new Vector3( x_pos, 0.0f, y_pos ), grid_object.transform.rotation );
		grid_first_object.layer = (int)collisionLayer.NEUTRAL;
		object_size = grid_first_object.GetComponent<Collider>().bounds.size;
		Vector3 objectToFillSize = objectToFill.GetComponent<Renderer>().bounds.size;
		x_elements = (int)( objectToFillSize.x/ (object_size.x * 1.5f) ) + 1;
		y_elements = (int)( objectToFillSize.z/ (object_size.y / 2 ) ) + 1;
		grid_first_object.transform.Translate( 0.0f, -object_size.z * 3/4 , 0.0f );
		
		for( int i = 0; i < y_elements; i++ )
		{
			if( i == 0 || i % 2 == 0 )
			{
				x_pos = startPos.x;
			}
			else
			{
				x_pos = object_size.x * 3/4;
			}
			
			for( int j = 0; j < x_elements; j++ )
			{
				GameObject grid_single_object = (GameObject)Instantiate( grid_object, new Vector3( x_pos, 0.0f, y_pos ), grid_object.transform.rotation );//-object_size.z * 3/4
				grid_single_object.transform.parent = objectToFill.transform.parent;
				grid_single_object.layer = (int)collisionLayer.NEUTRAL;
				grid_single_object.transform.parent = objectToFill.transform;
				
				x_pos += object_size.x * 1.5f;
			}
			
			y_pos -= object_size.y/2;
		}
	}
}
