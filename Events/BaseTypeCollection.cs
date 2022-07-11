using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace HomeSeer.PluginSdk.Events {

    /// <summary>
    /// A collection for holding types of classes that adhere to certain structural rules.
    /// </summary>
    /// <typeparam name="TBaseItemType">The type of item to manage</typeparam>
    /// <seealso cref="ActionTypeCollection"/>
    /// <seealso cref="TriggerTypeCollection"/>
    public class BaseTypeCollection<TBaseItemType> {

        /// <summary>
        /// The number of types that are supported by the collection
        /// </summary>
        public int Count => _itemTypes?.Count ?? 0;
        
        /// <summary>
        /// A <see cref="List{T}"/> of constructor signatures
        /// </summary>
        protected List<Type[]> ConstructorSignatures { get; set; } = new List<Type[]>();
        
        //Mode - All / One
        /// <summary>
        /// Whether the items in the collection must match all of the listed <see cref="ConstructorSignatures"/> or just one.
        ///  <see langword="true"/> if the items must match all constructor signatures.
        /// </summary>
        protected bool MatchAllSignatures { get; set; }
        
        /// <summary>
        /// <see cref="List{T}"/> of <see cref="Type">Types</see> that are a subclass of <typeparamref name="TBaseItemType"/>
        /// </summary>
        /// <see cref="AddItemType"/>
        protected List<Type>      _itemTypes     = new List<Type>();
        
        /// <summary>
        /// <see cref="HashSet{T}"/> of class names representing the <see cref="Type">Types</see> in <see cref="_itemTypes"/>
        /// </summary>
        private HashSet<string> _itemTypeNames = new HashSet<string>();

        /// <summary>
        /// Add the specified class type that derives from <typeparamref name="TBaseItemType"/> to the list of item types
        /// </summary>
        /// <param name="itemType">
        /// The <see cref="Type"/> of the class that derives from <typeparamref name="TBaseItemType"/>
        /// </param>
        /// <exception cref="ArgumentException">Thrown when the specified class type will not work as the desired type</exception>
        protected void AddItemType(Type itemType) {
            if (itemType?.FullName == null) {
                throw new ArgumentException(nameof(itemType));
            }
            if (!itemType.IsSubclassOf(typeof(TBaseItemType))) {
                throw new ArgumentException($"{itemType} is not derived from {typeof(TBaseItemType).FullName}", nameof(itemType));
            }
            AssertTypeHasConstructors(itemType);
            if (_itemTypeNames.Contains(itemType.FullName)) {
                throw new ArgumentException($"{itemType.FullName} has already been added to the collection.");
            }

            if (_itemTypeNames.Add(itemType.FullName)) {
                _itemTypes.Add(itemType);
            }
        }
        
        /// <summary>
        /// Get an instance of an object of the type defined at a specific index in the collection
        /// </summary>
        /// <param name="itemIndex">The index of the type to use</param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException">Thrown when no item is found at the specified index</exception>
        /// <exception cref="TypeLoadException">Thrown when there was an error while creating an instance of the specified type</exception>
        protected TBaseItemType GetObjectFromInfo(int itemIndex) {
            if (_itemTypes == null || _itemTypes.Count < itemIndex) {
                throw new KeyNotFoundException("No type exists with that number");
            }

            var targetType = _itemTypes[itemIndex];

            var typeConstructor = targetType.GetConstructor(BindingFlags.Instance | BindingFlags.Public,
                                                            null,
                                                            CallingConventions.Standard,
                                                            Type.EmptyTypes,
                                                            null);
            
            if (typeConstructor == null) {
                throw new TypeLoadException("Cannot find the correct constructor for this type");
            }
            
            if (!(typeConstructor.Invoke(Array.Empty<object>()) is TBaseItemType createdItem)) {
                throw new TypeLoadException($"This constructor did not produce a class derived from {typeof(TBaseItemType).FullName}");
            }

            return createdItem;
        }

        /// <inheritdoc cref="GetObjectFromInfo(int)"/>
        /// <param name="itemIndex">The index of the type to use</param>
        /// <param name="signatureIndex">The index of the constructor signature to use</param>
        /// <param name="inParams">The parameters to pass to the constructor</param>
        /// <exception cref="ArgumentException">Thrown when there was an issue with the provided parameters</exception>
        protected TBaseItemType GetObjectFromInfo(int itemIndex, int signatureIndex, params object[] inParams) {
            if (_itemTypes == null || _itemTypes.Count < itemIndex) {
                throw new KeyNotFoundException("No type exists with that number");
            }
            
            if (ConstructorSignatures == null || ConstructorSignatures.Count < signatureIndex) {
                throw new KeyNotFoundException("No constructor signature exists at that index");
            }

            var targetType = _itemTypes[itemIndex];
            var paramTypes = ConstructorSignatures[signatureIndex];
            if (paramTypes.Length != inParams.Length) {
                throw new ArgumentException("Target constructor and provided parameter counts do not match.");
            }
            
            for (var i = 0; i < inParams.Length; i++) {
                if (!paramTypes[i].IsInstanceOfType(inParams[i])) {
                    throw new ArgumentException($"Target constructor and provided parameter types do not match. Expecting {paramTypes[i].FullName} but got {inParams[i].GetType().FullName}");
                }
            }
            
            var typeConstructor = targetType.GetConstructor(BindingFlags.Instance | BindingFlags.Public,
                                                            null,
                                                            CallingConventions.Standard,
                                                            paramTypes,
                                                            null);
            
            if (typeConstructor == null) {
                throw new TypeLoadException("Cannot find the correct constructor for this type");
            }
            
            if (!(typeConstructor.Invoke(inParams) is TBaseItemType createdItem)) {
                throw new TypeLoadException($"This constructor did not produce a class derived from {typeof(TBaseItemType).FullName}");
            }

            return createdItem;
        }

        /// <summary>
        /// Determine if a specific type has a particular constructor signature defined
        /// </summary>
        /// <param name="itemIndex">The index of the type to use</param>
        /// <param name="signatureIndex">The index of the constructor signature to use</param>
        /// <returns><see langword="true"/> if the type has the specific constructor defined</returns>
        /// <exception cref="KeyNotFoundException">Thrown if a type or signature was not found at the specified index</exception>
        protected bool TypeHasConstructor(int itemIndex, int signatureIndex) {
            if (_itemTypes == null || _itemTypes.Count < itemIndex) {
                throw new KeyNotFoundException("No type exists with that number");
            }
            
            if (ConstructorSignatures == null || ConstructorSignatures.Count < signatureIndex) {
                throw new KeyNotFoundException("No constructor signature exists at that index");
            }

            var targetType = _itemTypes[itemIndex];
            var paramTypes = ConstructorSignatures[signatureIndex];
            try {
                AssertTypeHasConstructor(targetType, paramTypes);
                return true;
            }
            catch (Exception) {
                return false;
            }
        }
        
        private void AssertTypeHasConstructors(Type targetType) {

            if (ConstructorSignatures == null || ConstructorSignatures?.Count == 0) {
                throw new NullReferenceException("There are no ConstructorSignatures to check against so an assertion cannot be made.");
            }
            
            AssertTypeHasEmptyConstructor(targetType);

            var matchingSignatures = 0;

            foreach (var constructorSignature in ConstructorSignatures) {

                try {
                    AssertTypeHasConstructor(targetType, constructorSignature);
                    //Constructor match found
                    matchingSignatures++;
                }
                catch (TypeLoadException exception) {
                    //Constructor match not found
                    if (MatchAllSignatures) {
                        throw new TypeLoadException(exception.Message);
                    }
                }
                catch (Exception exception) {
                    #if DEBUG
                        Console.WriteLine($"Exception while checking for constructor with {constructorSignature.Length} params on {targetType.FullName} : {exception.Message} at {exception.StackTrace}");
                    #endif
                    throw;
                }
            }

            if (matchingSignatures == 0) {
                throw new Exception($"{GetType().FullName}: No matching constructor signatures implemented");
            }
        }

        private void AssertTypeHasEmptyConstructor(Type targetType) {

            try {
                AssertTypeHasConstructor(targetType, Type.EmptyTypes);
                //Constructor match found
            }
            catch (TypeLoadException) {
                //Constructor match not found
                var logMessage =
                    $"{GetType().FullName}: Type {targetType.FullName} must at least define a constructor with no parameters.";
                throw new TypeLoadException(logMessage);
            }
        }

        private static void AssertTypeHasConstructor(Type targetType, Type[] constructorParams) {
            var typeConstructor = targetType.GetConstructor(BindingFlags.Instance | BindingFlags.Public,
                                                            null,
                                                            CallingConventions.Standard,
                                                            constructorParams,
                                                            null);

            if (typeConstructor != null) {
                //Constructor match found
                return;
            }

            //Constructor match not found
            var logMessage = new StringBuilder("Type does not have a constructor with parameters ");
            foreach (var param in constructorParams) {
                logMessage.Append($"{param.FullName} ");
            }
            throw new TypeLoadException(logMessage.ToString());
        }
        

    }

}