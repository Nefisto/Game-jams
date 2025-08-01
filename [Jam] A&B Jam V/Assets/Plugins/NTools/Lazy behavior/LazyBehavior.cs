using System;
using UnityEngine;
using UnityEngine.UI;

public abstract partial class LazyBehavior : MonoBehaviour
{
	[NonSerialized]
    private Animator _animator;
    public Animator animator => _animator 
	    ? _animator
	    : _animator = GetComponent<Animator>();

    [NonSerialized]
    private SpriteRenderer _spriteRenderer;
    public SpriteRenderer spriteRenderer => _spriteRenderer 
		    ? _spriteRenderer 
            : _spriteRenderer = GetComponent<SpriteRenderer>();

    #region Physics

    [NonSerialized]
    private Rigidbody _rigidbody;

    public new Rigidbody rigidbody => _rigidbody
	    ? _rigidbody
	    : _rigidbody = GetComponent<Rigidbody>();

    [NonSerialized]
    private Rigidbody2D _rigidbody2D;

    public new Rigidbody2D rigidbody2D => _rigidbody2D 
	    ? _rigidbody2D 
	    : _rigidbody2D = GetComponent<Rigidbody2D>();

    [NonSerialized]
    private BoxCollider2D _boxCollider2D;

    public BoxCollider2D boxCollider2D => _boxCollider2D 
	    ? _boxCollider2D 
	    : _boxCollider2D = GetComponentInChildren<BoxCollider2D>();

    [NonSerialized]
    private CircleCollider2D _circleCollider2D;

    public CircleCollider2D circleCollider2D => _circleCollider2D 
	    ? _circleCollider2D
	    : _circleCollider2D = GetComponentInChildren<CircleCollider2D>();

    #endregion

    #region UI

    [NonSerialized]
    private Button _button;
    public Button button => _button
	    ? _button :
	    _button = GetComponent<Button>();

    [NonSerialized]
    private Image _image;

    public Image image => _image ?
	    _image :
	    _image = GetComponent<Image>();
    
    [NonSerialized]
    private Text _text;

    public Text text => _text ?
	    _text :
	    _text = GetComponent<Text>();

    #endregion
}