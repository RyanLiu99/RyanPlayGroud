﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SmallTests.Helpers;

namespace SmallTests.Entities
{
    /*
     JSON generated by Newtonsoft by default for ValueTuple<,> as entity Key
            {
              "e": [

                {
                  "Ids": [
                    {
                      "Item1": 1,
                      "Item2": "K1"
                    },
                    {
                      "Item1": 1,
                      "Item2": "K1"
                    },
                    {
                      "Item1": 2,
                      "Item2": "K1"
                    },
                    {
                      "Item1": 2,
                      "Item2": "K2"
                    }
                  ],
                  "EntityTypeName": "SmallTests.Entities.Person"
                }

              ],


              "c": [
                "Col1",
                "Col1",
                "Coll2"
              ]
            }

     */

    public class ValueDependencies
    {
        private List<EntityDependency>? _entityDependencies;
        private List<string>? _collectionDependencies;

        [JsonProperty("e")]
        public List<EntityDependency> EntityDependencies {
            get
            {
              return _entityDependencies ??= new List<EntityDependency>();
            }
        }

        [JsonProperty("c")]
        public List<string> CollectionDependencies
        {
            get { return _collectionDependencies ??= new List<string>(); }
        }

        public ValueDependencies()  // used as JsonConstructor
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityDependencies"></param>
        /// <param name="collectionDependencies"></param>
        /// <exception cref="ArgumentNullException"></exception>

        public ValueDependencies(IEnumerable<EntityDependency> entityDependencies, IEnumerable<string> collectionDependencies)
        {
            if(entityDependencies!= null) EntityDependencies.AddRange(entityDependencies);
            if(collectionDependencies!= null) CollectionDependencies.AddRange(collectionDependencies);
        }

        public ValueDependencies(IEnumerable<EntityDependency> entityDependencies, params string[] collectionDependencies)
        {
            if(entityDependencies!= null) EntityDependencies.AddRange(entityDependencies);
            ((List<string>)CollectionDependencies).AddRange(collectionDependencies);
        }

        public ValueDependencies(params EntityDependency[] entityDependencies) 
        {
            if (entityDependencies.IsNullOfEmpty()) return;
            ((List<EntityDependency>)EntityDependencies).AddRange(entityDependencies);
        }

        public ValueDependencies(params string[] collectionDependencies)
        {
            if (collectionDependencies.IsNullOfEmpty()) return;
            ((List<string>)CollectionDependencies).AddRange(collectionDependencies);
        }

        public bool IsEmpty() => _entityDependencies.IsNullOfEmpty() && _collectionDependencies.IsNullOfEmpty();

    }
}