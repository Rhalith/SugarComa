//using System;
//using UnityEditor;
//using UnityEngine;
//using System.Linq;
//using System.Collections.Generic;

//[CustomPropertyDrawer(typeof(PlatformDictionary))]
//public class PlatformDictionaryDrawer : PropertyDrawer
//{
//    private const float _HEIGHT = 18f;
//    private const float _BUTTON_WIDTH = 17f;
//    private const float _SPACE_ELEMENT = 1f;

//    private bool _mainFoldout = false;

//    private List<bool> _keyFoldouts;
//    private List<bool> _valueFoldouts;
//    private PlatformDictionary _platforms;
//    private Dictionary<int, List<bool>> _tempPlatforms;

//    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
//    {
//        Initialize(property, label);
//        return GetHeight();
//    }

//    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//    {
//        Initialize(property, label);

//        position.height = _HEIGHT;

//        // main foldout
//        AddFoldout(position, label.text, _mainFoldout, (changed) => _mainFoldout = changed);

//        // add new element button
//        AddButton(position, () => {
//            _tempPlatforms.Add(_tempPlatforms.Count, new List<bool>());
//            // _platforms.Add(new(), new());
//            _keyFoldouts.Add(false);
//            _valueFoldouts.Add(false);
//        });

//        // clear elements
//        ClearButton(position, () =>
//        {
//            _tempPlatforms.Clear();
//            _platforms.Clear();
//            _keyFoldouts.Clear();
//            _valueFoldouts.Clear();
//        });

//        if (!_mainFoldout) return;

//        int index = 0;

//        var elementRect = position;
//        elementRect.x += _BUTTON_WIDTH;
//        elementRect.width -= _BUTTON_WIDTH;
//        var layoutHeight = _HEIGHT + _SPACE_ELEMENT;

//        foreach (var tempPlatform in _tempPlatforms)
//        {
//            var tempKey = tempPlatform.Key;

//            var elementHeight = _HEIGHT + _SPACE_ELEMENT;

//            elementRect.y += layoutHeight;

//            // element foldout
//            var keyFouldout = _keyFoldouts[index];
//            AddFoldout(elementRect, $"Element {index}", keyFouldout, (changed) => _keyFoldouts[index] = changed);

//            // remove element
//            var keyRemoved = RemoveButton(elementRect, () =>
//            {
//                _tempPlatforms.Remove(tempKey);
//                if (index < _platforms.Count)
//                {
//                    var platform = _platforms.ElementAt(index);
//                    _platforms.Remove(platform.Key);
//                }
//            });
//            if (keyRemoved) break;

//            // element expanded
//            if (keyFouldout)
//            {
//                // Key
//                var keyRect = elementRect;
//                keyRect.y += _HEIGHT + _SPACE_ELEMENT;

//                Platform key = null;
//                List<Platform> value = null;

//                // if exists set key value pair values
//                if (index < _platforms.Count)
//                {
//                    var platform = _platforms.ElementAt(index);
//                    key = platform.Key;
//                    value = platform.Value;
//                }

//                var keyChanged = AddObject(keyRect, $"Key", key, (changed) =>
//                {
//                    if (key != null) _platforms.Remove(key);

//                    if (value == null) value = new List<Platform>();
//                    _platforms.Add(changed, value);

//                    key = changed;
//                });

//                if (keyChanged) break;

//                // value
//                if (value != null)
//                {
//                    var tempValue = _tempPlatforms[tempKey];

//                    var valueRect = keyRect;
//                    valueRect.y += _HEIGHT + _SPACE_ELEMENT;
//                    valueRect.x += 11f;
//                    valueRect.width -= 11f;

//                    // list foldout
//                    var valueFoldout = _valueFoldouts[index];
//                    AddFoldout(valueRect, $"Value", valueFoldout, (changed) => _valueFoldouts[index] = changed);

//                    // add new element button
//                    AddButton(valueRect, () => tempValue.Add(false));

//                    // clear elements
//                    ClearButton(valueRect, () => { _platforms[key].Clear(); tempValue.Clear(); });

//                    elementHeight += (_HEIGHT + _SPACE_ELEMENT) * 2f;
//                    if (valueFoldout)
//                    {
//                        elementHeight += (_HEIGHT + _SPACE_ELEMENT) * tempValue.Count;
//                        var nestedValueRect = valueRect;
//                        for (int i = 0; i < tempValue.Count; i++)
//                        {
//                            nestedValueRect.y += _HEIGHT + _SPACE_ELEMENT;

//                            Platform nestedValue = null;
//                            if (i < value.Count) nestedValue = value[i];
//                            AddObject(nestedValueRect, $"Element {i}", nestedValue, (changed) =>
//                            {
//                                if (i < value.Count) value[i] = changed;
//                                else value.Add(changed);
//                            });

//                            var nestedValueRemoved = RemoveButton(nestedValueRect, () => {
//                                if (i < value.Count) _platforms[key].RemoveAt(i);
//                                _tempPlatforms[tempKey].RemoveAt(i);
//                            });
//                            if (nestedValueRemoved) break;
//                        }
//                    }
//                }
//            }
//            layoutHeight = elementHeight;
//            index++;
//        }
//    }

//    private void Initialize(SerializedProperty property, GUIContent label)
//    {
//        if (_platforms == null)
//        {
//            _tempPlatforms = new();

//            var target = property.serializedObject.targetObject;

//            _platforms = fieldInfo.GetValue(target) as PlatformDictionary;
//            if (_platforms == null)
//            {
//                _platforms = new PlatformDictionary();
//                fieldInfo.SetValue(target, _platforms);
//            }
//            // initialize values
//            _keyFoldouts = new();
//            _valueFoldouts = new();

//            _mainFoldout = EditorPrefs.GetBool(label.text);
//            // initialize foldouts
//            int index = 0;
//            foreach (var platforms in _platforms.Values)
//            {
//                _keyFoldouts.Add(false);
//                _valueFoldouts.Add(false);

//                _tempPlatforms.Add(index, new());
//                for (int i = 0; i < platforms.Count; i++) _tempPlatforms[index].Add(false);
//                index++;
//            }
//        }
//    }

//    private void AddButton(Rect position, Action action)
//    {
//        var buttonRect = position;
//        buttonRect.x += position.width - _BUTTON_WIDTH;
//        buttonRect.width = _BUTTON_WIDTH + _SPACE_ELEMENT;
//        if (GUI.Button(buttonRect, new GUIContent("+", "Add item"), EditorStyles.miniButtonRight)) action.Invoke();
//    }

//    private bool RemoveButton(Rect position, Action action)
//    {
//        var buttonRect = position;
//        buttonRect.x += position.width - _BUTTON_WIDTH;
//        buttonRect.width = _BUTTON_WIDTH + _SPACE_ELEMENT;
//        if (GUI.Button(buttonRect, new GUIContent("-", "Remove item"), EditorStyles.miniButtonMid))
//        {
//            action.Invoke();
//            return true;
//        }
//        return false;
//    }

//    private void ClearButton(Rect position, Action action)
//    {
//        var buttonRect = position;
//        buttonRect.x += position.width - _BUTTON_WIDTH * 2f;
//        buttonRect.width = _BUTTON_WIDTH + _SPACE_ELEMENT;
//        if (GUI.Button(buttonRect, new GUIContent("x", "Clear items"), EditorStyles.miniButtonLeft)) action.Invoke();
//    }

//    private void AddFoldout(Rect position, string label, bool value, Action<bool> action)
//    {
//        EditorGUI.BeginChangeCheck();
//        var valueChanged = EditorGUI.Foldout(position, value, label);
//        if (EditorGUI.EndChangeCheck()) action.Invoke(valueChanged);
//    }

//    private bool AddObject<T>(Rect position, string label, T value, Action<T> action) where T : UnityEngine.Object
//    {
//        EditorGUI.BeginChangeCheck();
//        position.width -= _BUTTON_WIDTH + _SPACE_ELEMENT;
//        var valueChanged = (T)EditorGUI.ObjectField(position, label, value, typeof(T), true);
//        if (EditorGUI.EndChangeCheck())
//        {
//            action.Invoke(valueChanged);
//            return true;
//        }

//        return false;
//    }

//    private float GetHeight()
//    {
//        var height = _HEIGHT;

//        if (_mainFoldout)
//        {
//            height += (_HEIGHT + _SPACE_ELEMENT) * _keyFoldouts.Count;

//            int i = 0;
//            foreach (var el in _tempPlatforms)
//            {
//                if (_keyFoldouts[i])
//                {
//                    height += (_HEIGHT + _SPACE_ELEMENT) * 2f;
//                    if (_valueFoldouts[i] && el.Value.Count > 0)
//                        height += (_HEIGHT + _SPACE_ELEMENT) * el.Value.Count;
//                }

//                i++;
//            }
//        }
//        return height;
//    }
//}
