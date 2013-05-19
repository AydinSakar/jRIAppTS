﻿            
function defineProps(proto, props?: any, propertyDescriptors?:any) {
                var pds = propertyDescriptors || {}, propertyName;
                var pdsProperties = Object.getOwnPropertyNames(pds);
                pdsProperties.forEach(function (name) {
                    var pd = pds[name];
                    if (pd['enumerable'] === undefined) {
                        pd['enumerable'] = true;
                    }
                    if (pd['configurable'] === undefined) {
                        pd['configurable'] = false;
                    }
                });
              
                if (!!props) {
                    var simpleProperties = Object.getOwnPropertyNames(props);
                    for (var i = 0, len = simpleProperties.length; i < len; i += 1) {
                        propertyName = simpleProperties[i];
                        if (pds.hasOwnProperty(propertyName)) {
                            continue;
                        }

                        pds[propertyName] = Object.getOwnPropertyDescriptor(props, propertyName);
                    }
                }

                return Object.defineProperties(proto, pds);
            };


/*
      here we are testing how to extend my custom 'Defaults' class to a new 'Defaults2' class
      it will have two new properties, and two new functions
    */
    function __extendType(_super, pds, props) {
        function Defaults2() {
            _super.call(this);
        }
        //__extends is your function emitted after compilation in js file 
        __extends(Defaults2, _super);
        //adds properties in ES5 way
        defineProps(Defaults2.prototype, pds, props);
        return Defaults2;
    }

//replace RIAPP.GLOB.defaults.Defaults with some class definition available in your project
//the properties difinition is much more compact
//and there's no need to repeat Defaults2.prototype - we can produce much smaller js files in this way
    var Defaults2 = __extendType(RIAPP.GLOB.defaults.Defaults,
     {
         testFn1: function () { alert('test'); },
         testFn2: function (txt) { alert(txt); }
     },
     {
        testProperty1: {
            set: function (v) {
                this._dateTimeFormat = v;
            },
            get: function () {
                return this._dateTimeFormat;
            }
        },
        testProperty2: {
            set: function (v) {
                this._dateTimeFormat = v;
            },
            get: function () {
                return this._dateTimeFormat;
            }
        }
        
    });


    //debugger;
     var obj = new Defaults2();
     alert(obj.testProperty1);
     obj.testFn2('test testFn2');