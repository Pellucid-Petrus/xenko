﻿// Copyright (c) 2014 Silicon Studio Corp. (http://siliconstudio.co.jp)
// This file is distributed under GPL v3. See LICENSE.md for details.

using System.Collections.Generic;
using System.ComponentModel;
using SiliconStudio.Assets;
using SiliconStudio.Assets.Compiler;
using SiliconStudio.Core;
using SiliconStudio.Core.Annotations;
using SiliconStudio.Core.IO;
using SiliconStudio.Core.Yaml;
using SiliconStudio.Xenko.Graphics;
using SiliconStudio.Xenko.Graphics.Font;

namespace SiliconStudio.Xenko.Assets.SpriteFont
{
    /// <summary>
    /// Description of a font.
    /// </summary>
    [DataContract("SpriteFont")]
    [AssetDescription(FileExtension)]
    [AssetCompiler(typeof(SpriteFontAssetCompiler))]
    [AssetFormatVersion(XenkoConfig.PackageName, "1.7.0-beta02")]
    [AssetUpgrader(XenkoConfig.PackageName, "0.0.0", "1.5.0-alpha09", typeof(PremultiplyUpgrader))]
    [AssetUpgrader(XenkoConfig.PackageName, "1.5.0-alpha09", "1.7.0-beta02", typeof(FontTypeUpgrader))]    
    [Display(140, "Sprite Font")]
    [CategoryOrder(10, "Font")]
    [CategoryOrder(20, "Characters")]
    [CategoryOrder(30, "Rendering")]
    public class SpriteFontAsset : Asset
    {
        /// <summary>
        /// The default file extension used by the <see cref="SpriteFontAsset"/>.
        /// </summary>
        public const string FileExtension = ".xkfnt;.pdxfnt";

        [NotNull]
        [DataMember(10)]
        [Display(null, "Font Source")]
        public FontProviderBase FontSource { get; set; } = new SystemFontProvider();

        /// <summary>
        ///  Gets or sets the size in points of the font (ignored when converting a bitmap font).
        /// </summary>
        /// <userdoc>
        /// The size of the font (in points) for static fonts, the default size for dynamic fonts. This property is ignored when the font source is a bitmap.
        /// </userdoc>
        [DataMember(30)]
        [DefaultValue(16.0f)]
        [Display(null, "Font")]
        public float Size { get; set; } = 16.0f;

        /// <summary>
        ///  Gets or sets the value determining if and how the characters are pre-generated off-line or at run-time.
        /// </summary>
        /// <userdoc>
        /// Static font has fixed font size and is pre-compiled
        /// Dynamic font which can change its font size at runtime and is also compiled at runtime
        /// Signed Distance Field font is pre-compiled but can still be scaled at runtime
        /// </userdoc>
        [DataMember(50)]
        [Display(null, "Font")]
        public SpriteFontType FontType { get; set; } = SpriteFontType.Static;

        /// <summary>
        /// Gets or sets the fallback character used when asked to render a character that is not
        /// included in the font. If zero, missing characters throw exceptions.
        /// </summary>
        /// <userdoc>
        /// The fallback character to use when a given character is not available in the font file data.
        /// </userdoc>
        [DataMember(60)]
        [Display(null, "Characters")]
        public char DefaultCharacter { get; set; } = ' ';

        /// <summary>
        ///  Gets or sets the text file referencing which characters to include when generating the static fonts (eg. "ABCDEF...")
        /// </summary>
        /// <userdoc>
        /// The path to a file containing the characters to import from the font source file. This property is ignored when 'IsDynamic' is checked.
        /// </userdoc>
        [DataMember(70)]
        [Display(null, "Characters")]
        public UFile CharacterSet { get; set; } = new UFile("");

        /// <summary>
        /// Gets or set the additional character ranges to include when generating the static fonts (eg. "/CharacterRegion:0x20-0x7F /CharacterRegion:0x123")
        /// </summary>
        /// <userdoc>
        /// The list of series of character to import from the font source file. This property is ignored when 'IsDynamic' is checked.
        /// Note that this property only represents an alternative way of indicating character to import, the result is the same as using the 'CharacterSet' property.
        /// </userdoc>
        [DataMember(80)]
        [Category]
        [Display(null, "Characters")]
        [NotNullItems]
        public List<CharacterRegion> CharacterRegions { get; set; } = new List<CharacterRegion>();

        /// <summary>
        /// Gets or sets format of the texture used to render the font.
        /// </summary>
        /// <userdoc>
        /// The format of the texture used to render the Font. This property is currently ignored for dynamic fonts.
        /// </userdoc>
        [DataMember(100)]
        [DefaultValue(FontTextureFormat.Rgba32)]
        [Display(null, "Rendering")]
        public FontTextureFormat Format { get; set; } = FontTextureFormat.Rgba32;

        /// <summary>
        /// Gets or sets the font anti-aliasing mode. By default, levels of grays are used.
        /// </summary>
        /// <userdoc>
        /// The type of anti-aliasing to use when rendering the font. 
        /// </userdoc>
        [DataMember(110)]
        [Display(null, "Rendering")]
        public FontAntiAliasMode AntiAlias { get; set; } = FontAntiAliasMode.Default;

        /// <summary>
        /// Gets or sets the value indicating if the font texture should be generated pre-multiplied by alpha component. 
        /// </summary>
        /// <userdoc>
        /// If checked, the texture generated for this font is not pre-multiplied by the alpha component.
        /// Check this property if you prefer to use interpolative alpha blending when rendering the font.
        /// </userdoc>
        [DataMember(120)]
        [DefaultValue(true)]
        [Display("Premultiply", "Rendering")]
        public bool IsPremultiplied { get; set; } = true;

        /// <summary>
        /// Gets or sets the extra character spacing in pixels (relative to the font size). Zero is default spacing, negative closer together, positive further apart
        /// </summary>
        ///  <userdoc>
        /// The extra spacing to add between characters in pixels. Zero is default spacing, negative closer together, positive further apart.
        /// </userdoc>
        [DataMember(130)]
        [DefaultValue(0.0f)]
        [DataMemberRange(-500, 500, 1, 10)]
        [Display(null, "Rendering")]
        public float Spacing { get; set; }

        /// <summary>
        /// Gets or sets the extra line spacing in pixels (relative to the font size). Zero is default spacing, negative closer together, positive further apart.
        /// </summary>
        /// <userdoc>
        /// The extra interline space to add at each return of line (in pixels). Zero is default spacing, negative closer together, positive further apart.
        /// </userdoc>
        [DataMember(140)]
        [DefaultValue(0.0f)]
        [DataMemberRange(-500, 500, 1, 10)]
        [Display(null, "Rendering")]
        public float LineSpacing { get; set; }

        /// <summary>
        /// Gets or sets the factor to apply to the default line gap that separate each line. Default is <c>1.0f</c>
        /// </summary>
        /// <userdoc>
        /// The factor to use when calculating the LineGap of the font. 
        /// The LineGap affects both the space between two lines and the space at the top of the first line.
        /// </userdoc>
        [DataMember(150)]
        [DefaultValue(1.0f)]
        [DataMemberRange(-500, 500, 1, 10)]
        [Display(null, "Rendering")]
        public float LineGapFactor { get; set; } = 1.0f;

        /// <summary>
        /// Gets or sets the factor to apply to LineGap when calculating the font base line. See remarks. Default is <c>1.0f</c>
        /// </summary>
        /// <remarks>
        /// A Font total height = LineGap * LineGapFactor + Ascent + Descent
        /// A Font baseline = LineGap * LineGapFactor * LineGapBaseLineFactor + Ascent
        /// The <see cref="LineGapBaseLineFactor"/> specify where the line gap should start. A value of 1.0 means that the line gap
        /// should appear completely at the top of the line, while 0.0 would mean that the line gap would appear at the bottom
        /// of the line.
        /// </remarks>
        /// <userdoc>
        /// The factor to use when calculating the font base line. Moving the base line of font changes the repartition of the space at the top/bottom of the line.
        /// </userdoc>
        [DataMember(160)]
        [DefaultValue(1.0f)]
        [DataMemberRange(-500, 500, 1, 10)]
        [Display(null, "Rendering")]
        public float LineGapBaseLineFactor { get; set; } = 1.0f;

        /// <summary>
        /// Gets or sets the value specifying whether to use kerning information when rendering the font. Default value is false (NOT SUPPORTED YET).
        /// </summary>
        /// <userdoc>
        /// If checked, kerning information is imported from the font. (NOT SUPPORTED YET)
        /// </userdoc>
        [DataMember(170)]
        [Display(null, "Rendering")]
        public bool UseKerning { get; set; }

        internal string SafeCharacterSet => CharacterSet ?? "";

        class PremultiplyUpgrader : AssetUpgraderBase
        {
            protected override void UpgradeAsset(AssetMigrationContext context, PackageVersion currentVersion, PackageVersion targetVersion, dynamic asset, PackageLoadingAssetFile assetFile, OverrideUpgraderHint overrideHint)
            {
                if (asset.NoPremultiply != null)
                {
                    asset.IsPremultiplied = !(bool)asset.NoPremultiply;
                    asset.NoPremultiply = DynamicYamlEmpty.Default;
                }
                if (asset.IsNotPremultiply != null)
                {
                    asset.IsPremultiplied = !(bool)asset.IsNotPremultiply;
                    asset.IsNotPremultiply = DynamicYamlEmpty.Default;
                }
            }
        }

        class FontTypeUpgrader : AssetUpgraderBase
        {
            protected override void UpgradeAsset(AssetMigrationContext context, PackageVersion currentVersion, PackageVersion targetVersion, dynamic asset, PackageLoadingAssetFile assetFile,
                OverrideUpgraderHint overrideHint)
            {
                if (asset.IsDynamic != null)
                {
                    var isDynamic = (bool)asset.IsDynamic;

                    // There is also SDF type, but old assets don't have it yet
                    asset.AddChild("FontType", isDynamic ? "Dynamic" : "Static");

                    asset.RemoveChild("IsDynamic");
                }
            }

        }
    }
}
