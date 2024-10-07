﻿/*
The MIT License (MIT)
Copyright (c) 2018 Helix Toolkit contributors
*/
using Assimp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Animation = Assimp.Animation;
using Microsoft.Extensions.Logging;
using HelixToolkit.SharpDX.Core.Assimp;
using HelixToolkit.SharpDX.Core;
using Serilog.Core;

namespace HelixToolkit.SharpDX.Core
{
  using HxAnimations = Animations;
  using HxScene = Model.Scene;
  namespace Assimp
  {
    /// <summary>
    /// 
    /// </summary>
    public partial class Importer
    {
      /// <summary>
      /// Processes the node animation.
      /// </summary>
      /// <param name="channel">The channel.</param>
      /// <param name="ticksPerSecond">The ticks per second.</param>
      /// <param name="list">The list.</param>
      /// <returns></returns>
      protected virtual ErrorCode ProcessNodeAnimation( NodeAnimationChannel channel, double ticksPerSecond, out FastList<HxAnimations.Keyframe> list )
      {
        var posCount = channel.HasPositionKeys ? channel.PositionKeyCount : 0;
        var rotCount = channel.HasRotationKeys ? channel.RotationKeyCount : 0;
        var scaleCount = channel.HasScalingKeys ? channel.ScalingKeyCount : 0;
        int maxCount = Math.Max( posCount, Math.Max( rotCount, scaleCount ) );
        var ret = new FastList<HxAnimations.Keyframe>( maxCount );
        if ( posCount != rotCount || rotCount != scaleCount )
        {
          if ( logger.IsEnabled( LogLevel.Trace ) )
          {
            logger.LogTrace(
                "Animation Channel is non-uniform lengths. Position={0}; Rotation={1}; Scale={2};" +
                " Trying to automatically create uniform animation keys", posCount, rotCount, scaleCount );
          }
          // Adds dummy key if it is empty
          if ( posCount == 0 )
          {
            channel.PositionKeys.Add( new VectorKey( 0, new Vector3D() ) );
          }
          if ( rotCount == 0 )
          {
            channel.RotationKeys.Add( new QuaternionKey( 0, new Quaternion() ) );
          }
          if ( scaleCount == 0 )
          {
            channel.ScalingKeys.Add( new VectorKey( 0, new Vector3D( 1, 1, 1 ) ) );
          }
          int i = 0, j = 0, k = 0;
          double nextT1 = channel.PositionKeys[ i ].Time,
              nextT2 = channel.RotationKeys[ j ].Time,
              nextT3 = channel.ScalingKeys[ k ].Time;
          var minT = Math.Min( nextT1, Math.Min( nextT2, nextT3 ) );

          for ( int x = 0; x < maxCount && i < posCount && j < rotCount && k < scaleCount; ++x )
          {
            ret.Add( new HxAnimations.Keyframe()
            {
              Time = ( float ) ( minT / ticksPerSecond ),
              Translation = channel.PositionKeys[ i ].Value.ToSharpDXVector3(),
              Rotation = channel.RotationKeys[ j ].Value.ToSharpDXQuaternion(),
              Scale = channel.ScalingKeys[ k ].Value.ToSharpDXVector3(),
            } );
            nextT1 = ( i + 1 ) < posCount ? channel.PositionKeys[ i + 1 ].Time : double.MaxValue; // Set to max so index will not increase
            nextT2 = ( j + 1 ) < rotCount ? channel.RotationKeys[ j + 1 ].Time : double.MaxValue;
            nextT3 = ( k + 1 ) < scaleCount ? channel.ScalingKeys[ k + 1 ].Time : double.MaxValue;

            minT = Math.Min( nextT1, Math.Min( nextT2, nextT3 ) );
            if ( minT == nextT1 )
            {
              ++i;
            }
            if ( minT == nextT2 )
            {
              ++j;
            }
            if ( minT == nextT3 )
            {
              ++k;
            }
          }
        }
        else
        {
          for ( int i = 0; i < posCount; ++i )
          {
            ret.Add( new HxAnimations.Keyframe()
            {
              Time = ( float ) ( channel.PositionKeys[ i ].Time / ticksPerSecond ),
              Translation = channel.PositionKeys[ i ].Value.ToSharpDXVector3(),
              Rotation = channel.RotationKeys[ i ].Value.ToSharpDXQuaternion(),
              Scale = channel.ScalingKeys[ i ].Value.ToSharpDXVector3()
            } );
          }
        }

        list = ret;
        return ErrorCode.Succeed;
      }

      private ErrorCode LoadAnimations( HelixInternalScene scene )
      {
        var dict = new Dictionary<string, HxScene.SceneNode>( SceneNodes.Count );
        foreach ( var node in SceneNodes )
        {
          if ( node is HxScene.GroupNode && !dict.ContainsKey( node.Name ) )
          {
            dict.Add( node.Name, node );
          }
        }

        var nodeIdxDict = new Dictionary<string, int>();
        foreach ( var node in SceneNodes.Where( x => x is Animations.IBoneMatricesNode )
            .Select( x => x as Animations.IBoneMatricesNode ) )
        {
          if ( node.Bones != null )
          {
            nodeIdxDict.Clear();
            for ( var i = 0; i < node.Bones.Length; ++i )
            {
              nodeIdxDict.Add( node.Bones[ i ].Name, i );
            }
            for ( var i = 0; i < node.Bones.Length; ++i )
            {
              if ( dict.TryGetValue( node.Bones[ i ].Name, out var s ) )
              {
                ref var b = ref node.Bones[ i ];
                b.ParentNode = s.Parent;
                b.Node = s;
                b.BoneLocalTransform = s.ModelMatrix;
                if ( s.Parent != null && nodeIdxDict.TryGetValue( s.Parent.Name, out var idx ) )
                {
                  b.ParentIndex = idx;
                }
                s.IsAnimationNode = true; // Make sure to set this to true                                   
              }
            }

            if ( Configuration.CreateSkeletonForBoneSkinningMesh
                && node is HxScene.BoneSkinMeshNode sk
                && sk.Parent is HxScene.GroupNodeBase group )
            {
              var skeleton = sk.CreateSkeletonNode( Configuration.SkeletonMaterial,
                  Configuration.SkeletonEffects, Configuration.SkeletonSizeScale );
              skeleton.Name = "HxSK_" + sk.Name;
              group.AddChildNode( skeleton );
            }

            //Setup bone matrices initially if it's morphable (unable to render w/o bones)
            if ( node is HxScene.BoneSkinMeshNode sn
                && sn.MorphTargetWeights.Length > 0
                && sn.BoneMatrices?.Length == 0 )
            {
              sn.UpdateBoneMatrices();
            }
          }
        }

        if ( scene.AssimpScene.HasAnimations )
        {
          bool hasBoneSkinnedMesh = scene.Meshes.Where( x => x.Mesh is BoneSkinnedMeshGeometry3D ).Count() > 0 ? true : false;
          var animationList = new List<HxAnimations.Animation>( scene.AssimpScene.AnimationCount );
          if ( Configuration.EnableParallelProcessing )
          {
            Parallel.ForEach( scene.AssimpScene.Animations, ani =>
            {
              if ( LoadAnimation( ani, dict, hasBoneSkinnedMesh, out var hxAni ) == ErrorCode.Succeed )
              {
                lock ( animationList )
                {
                  animationList.Add( hxAni );
                }
              }

              if ( LoadMorphAnimation( ani, dict, hasBoneSkinnedMesh, out var hxMAni ) == ErrorCode.Succeed )
              {
                lock ( animationList )
                {
                  animationList.AddRange( hxMAni );
                }
              }
            } );
          }
          else
          {
            foreach ( var ani in scene.AssimpScene.Animations )
            {
              if ( LoadAnimation( ani, dict, hasBoneSkinnedMesh, out var hxAni ) == ErrorCode.Succeed )
                animationList.Add( hxAni );
              if ( LoadMorphAnimation( ani, dict, hasBoneSkinnedMesh, out var hxMAni ) == ErrorCode.Succeed )
                animationList.AddRange( hxMAni );
            }
          }
          scene.Animations = animationList;
          Animations.AddRange( animationList );
        }
        return ErrorCode.Succeed;
      }

      private ErrorCode LoadAnimation( Animation ani, Dictionary<string, HxScene.SceneNode> dict, bool searchBoneSkinMeshNode,
          out HxAnimations.Animation hxAni )
      {
        if ( ani.TicksPerSecond == 0 )
        {
          logger.LogWarning( "Animation TicksPerSecond is 0. Set to {0}", configuration.TickesPerSecond );
          ani.TicksPerSecond = configuration.TickesPerSecond;
        }
        hxAni = new HxAnimations.Animation( HxAnimations.AnimationType.Node )
        {
          StartTime = 0,
          EndTime = ( float ) ( ani.DurationInTicks / ani.TicksPerSecond ),
          Name = ani.Name,
          NodeAnimationCollection = new List<HxAnimations.NodeAnimation>( ani.NodeAnimationChannelCount )
        };

        if ( ani.HasNodeAnimations )
        {
          var code = ErrorCode.None;
          foreach ( var key in ani.NodeAnimationChannels )
          {
            System.Diagnostics.Debug.WriteLine( key.NodeName );
            if ( dict.TryGetValue( key.NodeName, out var node ) )
            {
              var nAni = new HxAnimations.NodeAnimation
              {
                Node = node
              };

              node.IsAnimationNode = true;// Make sure to set this to true
              code = ProcessNodeAnimation( key, ani.TicksPerSecond, out var keyframes );
              if ( code == ErrorCode.Succeed )
              {
                nAni.KeyFrames = keyframes;
                hxAni.NodeAnimationCollection.Add( nAni );
              }
              else
              {
                break;
              }
            }
          }
          if ( searchBoneSkinMeshNode )
          {
            FindBoneSkinMeshes( hxAni );
          }
          return code;
        }

        return ErrorCode.Failed;
      }

      private ErrorCode LoadMorphAnimation( Animation ani, Dictionary<string, HxScene.SceneNode> dict, bool searchBoneSkinMeshNode,
          out List<HxAnimations.Animation> hxAnis )
      {
        if ( ani.TicksPerSecond == 0 )
        {
          logger.LogWarning( "Animation TicksPerSecond is 0. Set to {0}", configuration.TickesPerSecond );
          ani.TicksPerSecond = configuration.TickesPerSecond;
        }

        hxAnis = new List<HxAnimations.Animation>();
        if ( ani.MeshMorphAnimationChannelCount > 0 )
        {
          foreach ( MeshMorphAnimationChannel aniChannel in ani.MeshMorphAnimationChannels )
          {
            HxAnimations.Animation hxAni = new HxAnimations.Animation( HxAnimations.AnimationType.MorphTarget )
            {
              StartTime = 0,
              EndTime = ( float ) ( ani.DurationInTicks / ani.TicksPerSecond ),
              Name = ani.Name,
              MorphTargetKeyframes = new List<HxAnimations.MorphTargetKeyframe>()
            };

            //Reference node (removes "*0", i don't know why but its there sometimes)
            string nodeName = aniChannel.Name.Replace( "*0", "" );
            if ( dict.TryGetValue( nodeName, out var node ) )
              hxAni.RootNode = node.Items.Where( i => ( i as HxScene.BoneSkinMeshNode ).MorphTargetWeights?.Length > 0 ).FirstOrDefault();
            else
              continue;

            //Add keyframes
            foreach ( var key in aniChannel.MeshMorphKeys )
            {
              for ( int i = 0; i < key.Values.Count; i++ )
              {
                hxAni.MorphTargetKeyframes.Add( new HxAnimations.MorphTargetKeyframe()
                {
                  Index = key.Values[ i ],
                  Weight = ( float ) key.Weights[ i ],
                  Time = ( float ) key.Time / ( float ) ani.TicksPerSecond
                } );
              }
            }
            if ( searchBoneSkinMeshNode )
            {
              FindBoneSkinMeshes( hxAni );
            }
            hxAnis.Add( hxAni );
          }

          return ErrorCode.Succeed;
        }

        return ErrorCode.Failed;
      }

      private void FindBoneSkinMeshes( HxAnimations.Animation animation )
      {
        if ( animation.NodeAnimationCollection != null && animation.NodeAnimationCollection.Count > 0 )
        {
          // Search all the bone skinned meshes from the common animation node root
          var node = animation.NodeAnimationCollection[ 0 ].Node;
          while ( node != null && !node.IsAnimationNodeRoot )
          {
            node = node.Parent;
          }

          if ( node == null )
          {
            return;
          }

          if ( node.Parent != null )
            node = node.Parent;
          animation.BoneSkinMeshes = new List<Animations.IBoneMatricesNode>();
          animation.RootNode = node;
          foreach ( var n in SceneNodes[ 0 ].Items.PreorderDFT( ( m ) => { return true; } ) )
          {
            if ( n is Animations.IBoneMatricesNode boneNode )
            {
              animation.BoneSkinMeshes.Add( boneNode );
            }
          }
        }
        else if ( animation.MorphTargetKeyframes != null && animation.MorphTargetKeyframes.Count > 0 )
        {
          animation.BoneSkinMeshes = new List<HxAnimations.IBoneMatricesNode>();
          if ( animation.RootNode is HxAnimations.IBoneMatricesNode bnode )
          {
            animation.BoneSkinMeshes.Add( bnode );
          }
        }
      }
    }
  }
}