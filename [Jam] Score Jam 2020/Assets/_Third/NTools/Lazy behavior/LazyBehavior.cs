using System;
using UnityEngine;
using UnityEngine.UI;

public abstract partial class LazyBehavior : MonoBehaviour
{
    [HideInInspector, NonSerialized]
    private Rigidbody _rigidbody;
    public new Rigidbody rigidbody { get => _rigidbody ? _rigidbody : (_rigidbody = GetComponent<Rigidbody>()); }

    [HideInInspector, NonSerialized]
    private Rigidbody2D _rigidbody2D;
    public new Rigidbody2D rigidbody2D { get => _rigidbody2D ? _rigidbody2D : (_rigidbody2D = GetComponent<Rigidbody2D>()); }

    [HideInInspector, NonSerialized]
    private Animator _animator;
    public Animator animator { get => _animator ? _animator : (_animator = GetComponent<Animator>()); }

    [HideInInspector, NonSerialized]
    private SpriteRenderer _spriteRenderer;
    public SpriteRenderer spriteRenderer { get => _spriteRenderer ? _spriteRenderer : (_spriteRenderer = GetComponent<SpriteRenderer>()); }

    [HideInInspector, NonSerialized]
    private BoxCollider2D _boxCollider2D;
    public BoxCollider2D boxCollider2D { get => _boxCollider2D ? _boxCollider2D : (_boxCollider2D = GetComponentInChildren<BoxCollider2D>()); }

    [HideInInspector, NonSerialized]
    private CircleCollider2D _circleCollider2D;
    public CircleCollider2D circleCollider2D { get => _circleCollider2D ? _circleCollider2D : (_circleCollider2D = GetComponentInChildren<CircleCollider2D>()); }
	
    [HideInInspector, NonSerialized]
    private CapsuleCollider2D _capsuleCollider2D;
    public CapsuleCollider2D  capsuleCollider2D { get => _capsuleCollider2D ? _capsuleCollider2D : (_capsuleCollider2D = GetComponent<CapsuleCollider2D>()); }

    [HideInInspector, NonSerialized] 
    private AudioSource _audioSource;
    public AudioSource audioSource
    {
        get => _audioSource ? _audioSource : (_audioSource = GetComponent<AudioSource>());
    }
    

    [HideInInspector, NonSerialized]
    private Image _image;
    public Image image { get => _image ? _image : (_image = GetComponent<Image>()); }

    [HideInInspector, NonSerialized]
    private Button _button;
    public Button button { get => _button ? _button : (_button = GetComponent<Button>()); }

    [HideInInspector, NonSerialized]
    private Text _text;
    public Text text { get => _text ? _text : (_text = GetComponent<Text>()); }
}
