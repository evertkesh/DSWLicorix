---
name: Licorix Design System
colors:
  surface: '#fbf9f8'
  surface-dim: '#dbdad9'
  surface-bright: '#fbf9f8'
  surface-container-lowest: '#ffffff'
  surface-container-low: '#f5f3f3'
  surface-container: '#efeded'
  surface-container-high: '#e9e8e7'
  surface-container-highest: '#e4e2e2'
  on-surface: '#1b1c1c'
  on-surface-variant: '#444748'
  inverse-surface: '#303031'
  inverse-on-surface: '#f2f0f0'
  outline: '#747878'
  outline-variant: '#c4c7c7'
  surface-tint: '#5f5e5e'
  primary: '#000000'
  on-primary: '#ffffff'
  primary-container: '#1c1b1b'
  on-primary-container: '#858383'
  inverse-primary: '#c8c6c5'
  secondary: '#735c00'
  on-secondary: '#ffffff'
  secondary-container: '#fed65b'
  on-secondary-container: '#745c00'
  tertiary: '#000000'
  on-tertiary: '#ffffff'
  tertiary-container: '#1a1c1b'
  on-tertiary-container: '#838482'
  error: '#ba1a1a'
  on-error: '#ffffff'
  error-container: '#ffdad6'
  on-error-container: '#93000a'
  primary-fixed: '#e5e2e1'
  primary-fixed-dim: '#c8c6c5'
  on-primary-fixed: '#1c1b1b'
  on-primary-fixed-variant: '#474646'
  secondary-fixed: '#ffe088'
  secondary-fixed-dim: '#e9c349'
  on-secondary-fixed: '#241a00'
  on-secondary-fixed-variant: '#574500'
  tertiary-fixed: '#e3e2e0'
  tertiary-fixed-dim: '#c7c6c5'
  on-tertiary-fixed: '#1a1c1b'
  on-tertiary-fixed-variant: '#464746'
  background: '#fbf9f8'
  on-background: '#1b1c1c'
  surface-variant: '#e4e2e2'
typography:
  display-lg:
    fontFamily: Libre Caslon Text
    fontSize: 48px
    fontWeight: '400'
    lineHeight: '1.1'
    letterSpacing: -0.02em
  display-lg-mobile:
    fontFamily: Libre Caslon Text
    fontSize: 36px
    fontWeight: '400'
    lineHeight: '1.2'
  headline-md:
    fontFamily: Libre Caslon Text
    fontSize: 32px
    fontWeight: '400'
    lineHeight: '1.3'
  headline-sm:
    fontFamily: Libre Caslon Text
    fontSize: 24px
    fontWeight: '400'
    lineHeight: '1.4'
  body-lg:
    fontFamily: Hanken Grotesk
    fontSize: 18px
    fontWeight: '400'
    lineHeight: '1.6'
  body-md:
    fontFamily: Hanken Grotesk
    fontSize: 16px
    fontWeight: '400'
    lineHeight: '1.6'
  label-caps:
    fontFamily: Hanken Grotesk
    fontSize: 12px
    fontWeight: '600'
    lineHeight: '1'
    letterSpacing: 0.1em
spacing:
  base: 8px
  container-max: 1280px
  gutter: 24px
  margin-mobile: 16px
  margin-desktop: 64px
---

## Brand & Style
The design system for this premium spirits platform is built on a foundation of **Modern Minimalism with Luxury Editorial influences**. It balances the authoritative presence of a high-end concierge with the streamlined efficiency of contemporary e-commerce. 

The aesthetic is characterized by expansive whitespace, precise grid alignment, and a sophisticated interplay between traditional serif elegance and functional sans-serif clarity. The visual narrative avoids excessive decoration, instead relying on high-quality product photography and a restrained, high-contrast color application to evoke a sense of exclusivity and curation.

## Colors
The palette is rooted in a "Noir & Gold" philosophy. 

- **Primary (Deep Charcoal):** Used for primary text, iconography, and structural headers to provide a grounded, premium weight.
- **Secondary (Elegant Gold):** Reserved for highlights, primary call-to-actions, and brand accents. It should be used sparingly to maintain its impact.
- **Background & Surfaces:** A "Clean White" (#FFFFFF) serves as the primary canvas, with "Tertiary Off-White" (#F9F8F6) used for section subtle differentiation and container backgrounds.
- **Status Colors:** Functional indicators use desaturated, "Stock Green" and "Warning Red" to provide utility without breaking the sophisticated aesthetic.

## Typography
The typographic scale utilizes a dual-font strategy to bridge the gap between heritage and technology.

- **Headlines:** Use *Libre Caslon Text*. This serif face communicates the "aged" quality of premium spirits and provides an editorial feel. It should be used for product titles, section headers, and promotional banners.
- **Body & Interface:** Use *Hanken Grotesk*. This sans-serif is used for all functional text, product descriptions, and navigation. Its high legibility and contemporary geometry ensure a seamless shopping experience.
- **Micro-copy:** Use `label-caps` for inventory status, categories, and breadcrumbs to create a distinct visual hierarchy.

## Layout & Spacing
The design system utilizes a **Fixed Grid** on desktop (12 columns) and a **Fluid Grid** on mobile (4 columns). 

- **Rhythm:** A base-8 spacing scale drives all padding and margins. 
- **Product Grids:** Maintain generous gutters (24px) to allow product imagery to breathe. 
- **Alignment:** All content follows a strict vertical rhythm. Information-heavy views like the shopping cart table should prioritize horizontal scan-lines with 16px row padding.
- **Responsive Behavior:** On tablet/mobile, sidebars (filters) should transition into a bottom-sheet or full-screen overlay to maximize screen real estate for product browsing.

## Elevation & Depth
Depth is expressed through **Tonal Layers** and **Low-Contrast Outlines** rather than aggressive shadows.

1.  **Level 0 (Base):** The primary background (#FFFFFF).
2.  **Level 1 (Surface):** Subtle containers or cards using a 1px border (#E5E5E5). No shadow.
3.  **Level 2 (Interaction):** On hover, cards may lift slightly using a very soft, ambient shadow (10% opacity, 20px blur, 0px offset) to indicate interactivity.
4.  **Overlays:** Modals and shopping carts use a semi-transparent backdrop blur (8px) to maintain context while focusing user attention.

## Shapes
To maintain a high-end, "Architectural" feel, the system uses **Sharp (0px)** corners for primary UI elements.

- **Product Cards:** Strict 90-degree angles.
- **Buttons:** Sharp corners convey a sense of precision and premium tailoring.
- **Inputs:** Squared-off borders with 1px thickness.
- **Exceptions:** Very small functional icons (e.g., "remove from cart" 'x') may use circular backgrounds to provide visual variety, but all structural components remain geometric and sharp.

## Components

### Product Cards
The centerpiece of the e-commerce experience. Features a large, high-resolution image on a Tertiary background (#F9F8F6), followed by the product name in *Libre Caslon Text* and the price in *Hanken Grotesk* Bold. The inventory status (Stock Green) sits at the top right in `label-caps`.

### Buttons
- **Primary:** Solid Deep Charcoal background with White text. Sharp corners.
- **Secondary:** Transparent background, Deep Charcoal 1px border.
- **Action/CTA:** Gold (#D4AF37) background for "Checkout" or "Limited Edition" highlights.

### Filter Sidebar
A clean, vertical list utilizing accordion headers. Use checkboxes with sharp corners. Active filters are highlighted with a subtle Gold underline or dot.

### Shopping Cart Table
A minimalist table structure. Rows are separated by light 1px lines (#E5E5E5). Typography is kept small (14px-16px) to handle data-heavy orders. The "Total" section uses *Libre Caslon Text* for the final amount to add a premium touch.

### Real-Time Status Timeline
For order tracking, use a vertical line (1px Gold) with small Charcoal square nodes. Each node represents a status (Ordered, Dispatched, Out for Delivery, Delivered). Completed stages use the Gold accent color.

### Input Fields
Bottom-border only or full-boxed 1px Charcoal border. Floating labels using `label-caps` typography style when active.