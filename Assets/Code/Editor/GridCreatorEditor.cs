using UnityEditor;
using UnityEngine;

public class GridCreatorEditor: EditorWindow
{
	public GameObject grid_object; // Single grid object
	public GameObject objectToFill = null;

	// We need a start position	
	private Vector3 object_size = Vector3.zero;
	private int x_elements = 0;
	private int y_elements = 0;

	[MenuItem("Window/GridCreator")]
	public static void ShowWindow()
	{
		//Show existing window instance. If one doesn't exist, make one.
		EditorWindow.GetWindow(typeof(GridCreatorEditor));
	}
	
	void OnGUI()
	{
		objectToFill = EditorGUI.ObjectField(new Rect(3,33,position.width - 6, 20),
		                            "Object to fill",
		                                     objectToFill, typeof(GameObject), true ) as GameObject;

		grid_object = EditorGUI.ObjectField( new Rect( 3, 60, position.width - 6, 20 ),
		                                   "grid object", grid_object, typeof( GameObject ), true ) as GameObject;

		if( GUILayout.Button( "Generate" ) )
		{
			createGrid();
		}
	}

	public void createGrid()
	{
		float x_pos = 0.0f;
		float y_pos = 0.0f;
		
		// Define the amount of elements we need.
		GameObject grid_first_object = (GameObject)Instantiate( grid_object, new Vector3( x_pos, 0.0f, y_pos ), grid_object.transform.rotation );
		grid_first_object.layer = (int)GridCreator.collisionLayer.NEUTRAL;
		object_size = grid_first_object.GetComponent<Collider>().bounds.size;
		Vector3 objectToFillSize = objectToFill.GetComponent<Renderer>().bounds.size;
		x_elements = (int)( objectToFillSize.x/ (object_size.x * 1.5f) ) + 1;
		y_elements = (int)( objectToFillSize.z/ (object_size.y / 2 ) ) + 1;
		grid_first_object.transform.Translate( 0.0f, -object_size.z * 3/4 , 0.0f );
		
		for( int i = 0; i < y_elements; i++ )
		{
			if( i == 0 || i % 2 == 0 )
			{
				x_pos = 0.0f;
			}
			else
			{
				x_pos = object_size.x * 3/4;
			}
			
			for( int j = 0; j < x_elements; j++ )
			{
				GameObject grid_single_object = (GameObject)Instantiate( grid_object, new Vector3( x_pos, 0.0f, y_pos ), grid_object.transform.rotation );//-object_size.z * 3/4
				grid_single_object.transform.parent = objectToFill.transform.parent;
				grid_single_object.layer = (int)GridCreator.collisionLayer.NEUTRAL;
				grid_single_object.transform.parent = objectToFill.transform;
				
				x_pos += object_size.x * 1.5f;
			}
			
			y_pos -= object_size.y/2;
		}
	}
}
