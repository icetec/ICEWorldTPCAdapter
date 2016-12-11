// ##############################################################################
//
// ICEWorldTPCAdapter.cs
// Version 1.3.5
//
// © Pit Vetterick, ICE Technologies Consulting LTD. All Rights Reserved.
// http://www.ice-technologies.com
// mailto:support@ice-technologies.com
// 
// Unity Asset Store End User License Agreement (EULA)
// http://unity3d.com/legal/as_terms
//
// ##############################################################################

using UnityEngine;
using System.Collections;

using ICE;
using ICE.World;
using ICE.World.Objects;
using ICE.World.Utilities;
using ICE.World.EnumTypes;

using Opsive.ThirdPersonController;

namespace ICE.World.Adapter
{
	[RequireComponent (typeof (ICEWorldEntity))]
	public class ICEWorldTPCAdapter : Health 
	{
		protected ICEWorldEntity m_Entity = null;
		public ICEWorldEntity Entity{
			get{ return m_Entity = ( m_Entity == null ? ICEWorldEntity.GetWorldEntity( this.gameObject ) : m_Entity ); }
		}
		/**/
		protected override void Awake()
		{
			base.Awake();

			if( Entity != null )
			{
				MaxHealth = Entity.Status.InitialDurability;
				SetHealthAmount( Entity.Status.Durability );
			}
		}


		public override void Damage( float _amount, Vector3 _position, Vector3 _force, float _radius, GameObject _attacker, GameObject _target )
		{
			if( Entity == null )
				return;

			_target = ( _target != null ? _target : gameObject );
			_attacker = ( _attacker != null ? _attacker : gameObject );

			Vector3 _direction = ( _attacker != null && _target != null ? _attacker.transform.position - _target.transform.position : _force );
			_direction.Normalize();

			Entity.AddDamage( _amount, _direction, _position, _attacker.transform, _force.magnitude );

			if( ! Entity.Status.IsDestroyed )
				base.Damage( _amount, _position, _force, _radius, _attacker, _target );
			else
				base.Die( _position, _force, _attacker );
		}
	}
}
