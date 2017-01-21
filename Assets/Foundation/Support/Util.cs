using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Util 
{
	public static T DeepClone<T>( T obj )
	{
		using( var ms = new System.IO.MemoryStream() )
		{
			var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
			formatter.Serialize( ms, obj );
			ms.Position = 0;

			return (T)formatter.Deserialize(ms);
		}
	}

	private static Dictionary< int, List< Vector2 > > _cachedCosSins = new Dictionary< int, List< Vector2 > >();
	public static Vector2 CosSinCachedBySteps( int id, int n )
	{
		if( !_cachedCosSins.ContainsKey( n ) )
		{
			List< Vector2 > injected = new List< Vector2 >();
			for( int i=0; i<n; ++i )
			{
				float theta = (Mathf.PI / n) * (2 * i);
				injected.Add( new Vector2( Mathf.Cos( theta ), Mathf.Sin( theta ) ) );
			}
			_cachedCosSins[n] = injected;
		}

		return _cachedCosSins[n][id % n];
	}

	public static Component GetComponentOfTypeOnGameObject<T>( GameObject gameObject ) where T:class
	{
		Component[] components = gameObject.GetComponents( typeof( Component ) );
		for( int i=0; i<components.Length; ++i )
		{
			if( components[i] is T )
			{
				return components[i];
			}
		}
		return null;
	}
}
