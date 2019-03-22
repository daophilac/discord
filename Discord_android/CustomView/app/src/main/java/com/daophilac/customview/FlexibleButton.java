package com.daophilac.customview;

import android.content.Context;
import android.content.res.TypedArray;
import android.graphics.Canvas;
import android.graphics.Color;
import android.graphics.Paint;
import android.graphics.Rect;
import android.support.annotation.Nullable;
import android.util.AttributeSet;
import android.view.View;

class Shape{
    static final int RECTANGLE = 0;
    static final int SQUARE = 1;
    static final int CIRCLE = 2;
}
class TextHorizontalAlign{
    static final int LEFT = 0;
    static final int RIGHT = 1;
    static final int CENTER = 2;
}
class TextVerticalAlign{
    static final int TOP = 0;
    static final int BOTTOM = 1;
    static final int CENTER = 2;
}
class DrawableScaleMode {
    static final int ORIGIN = 0;
    static final int CROP_FIT = 1;
    static final int SCALEXY_FIT = 2;
    static final int SCALE_ASPECT_RATIO = 3;
}

class Default{
    static final boolean CENTER_PARENT = false;
    static final Paint.Style PAINT_STYLE = Paint.Style.FILL;
    static final int SHAPE = Shape.RECTANGLE;
    static final int SHAPE_COLOR = Color.GREEN;
    static final int TEXT_SIZE = 32;
    static final int TEXT_PADDING = 20;
    static final int TEXT_PADDING_LEFT = 10;
    static final int TEXT_PADDING_TOP = 10;
    static final int TEXT_PADDING_RIGHT = 10;
    static final int TEXT_PADDING_BOTTOM = 10;
    static final int TEXT_HORIZONTAL_ALIGN = TextHorizontalAlign.CENTER;
    static final int TEXT_VERTICAL_ALIGN = TextVerticalAlign.CENTER;
    static final int TEXT_COLOR = Color.BLACK;
    static final int NO_TEXT_WIDTH = 300;
    static final int NO_TEXT_HEIGHT = 80;
    static final int DRAWABLE_SCALE_MODE = DrawableScaleMode.ORIGIN;

}

public class FlexibleButton extends View {
    private boolean centerParent;
    private Paint shapePaint;
    private Paint textPaint;
    private Rect textBound;
    //private Rect rect;              //TODO: This one is redundant
    private String text;
    private boolean hasText;
    private int textSize;
    private int textPadding;
    private int textPaddingLeft;
    private int textPaddingTop;
    private int textPaddingRight;
    private int textPaddingBottom;
    private int textHorizontalAlign;
    private int textVerticalAlign;
    private int shape;
    private int drawableScaleMode;


    private int textWidth;
    private int textHeight;
    private int actualWidth;
    private int actualHeight;
    private int increaseWidthAmount = 0;
    private int increaseHeightAmount = 0;


    public FlexibleButton(Context context) {
        super(context);
        init(null);
    }

    public FlexibleButton(Context context, AttributeSet attrs) {
        super(context, attrs);
        init(attrs);
    }

    public FlexibleButton(Context context, AttributeSet attrs, int defStyleAttr) {
        super(context, attrs, defStyleAttr);
        init(attrs);
    }

    public FlexibleButton(Context context, AttributeSet attrs, int defStyleAttr, int defStyleRes) {
        super(context, attrs, defStyleAttr, defStyleRes);
        init(attrs);
    }

    public void init(@Nullable AttributeSet set){
        if(set == null){
            return;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////
        TypedArray typedArray = getContext().obtainStyledAttributes(set, R.styleable.FlexibleButton);
        this.shapePaint = new Paint(Paint.ANTI_ALIAS_FLAG);
        this.textPaint = new Paint(Paint.ANTI_ALIAS_FLAG);
        this.textBound = new Rect();
        this.centerParent = typedArray.getBoolean(R.styleable.FlexibleButton_centerParent, Default.CENTER_PARENT);
        this.shape = typedArray.getInt(R.styleable.FlexibleButton_shape, Default.SHAPE);
        this.text = typedArray.getString(R.styleable.FlexibleButton_text);
        this.textSize = typedArray.getInt(R.styleable.FlexibleButton_text_size, Default.TEXT_SIZE);
        this.textPadding = typedArray.getInt(R.styleable.FlexibleButton_text_padding, Default.TEXT_PADDING);
        this.textPaddingLeft = typedArray.getInt(R.styleable.FlexibleButton_text_paddingLeft, Default.TEXT_PADDING_LEFT);
        this.textPaddingTop = typedArray.getInt(R.styleable.FlexibleButton_text_paddingTop, Default.TEXT_PADDING_TOP);
        this.textPaddingRight = typedArray.getInt(R.styleable.FlexibleButton_text_paddingRight, Default.TEXT_PADDING_RIGHT);
        this.textPaddingBottom = typedArray.getInt(R.styleable.FlexibleButton_text_paddingBottom, Default.TEXT_PADDING_BOTTOM);
        this.textHorizontalAlign = typedArray.getInt(R.styleable.FlexibleButton_text_horizontalAlign, Default.TEXT_HORIZONTAL_ALIGN);
        this.textVerticalAlign = typedArray.getInt(R.styleable.FlexibleButton_text_verticalAlign, Default.TEXT_VERTICAL_ALIGN);
        this.drawableScaleMode = typedArray.getInt(R.styleable.FlexibleButton_drawable_scaleMode, Default.DRAWABLE_SCALE_MODE);

        /////////////////////////////////////////////////////////////////////////////////////////////
        this.shapePaint.setStyle(Default.PAINT_STYLE);
        this.shapePaint.setColor(typedArray.getColor(R.styleable.FlexibleButton_shape_color, Default.SHAPE_COLOR));
        this.textPaint.setTextSize(this.textSize);
        this.textPaint.setColor(typedArray.getColor(R.styleable.FlexibleButton_text_color, Default.TEXT_COLOR));

        /////////////////////////////////////////////////////////////////
        if(typedArray.hasValue(R.styleable.FlexibleButton_text_padding)){
            this.textPaddingLeft = this.textPadding;
            this.textPaddingTop = this.textPadding;
            this.textPaddingRight = this.textPadding;
            this.textPaddingBottom = this.textPadding;
        }
        if(this.text != null){
            this.hasText = true;
        }
        else{
            this.hasText = false;
        }
        typedArray.recycle();
    }

    @Override
    protected void onDraw(Canvas canvas) {
        super.onDraw(canvas);
        switch(this.shape){
            case Shape.RECTANGLE:
                drawRectangleButton(canvas);
                break;
            case Shape.CIRCLE:
                drawCircleButton(canvas);
                break;
        }
    }
    protected void drawRectangleButton(Canvas canvas){
        if(this.hasText){
            int xPosition;
            int yPosition;
            if(this.textHorizontalAlign == TextHorizontalAlign.LEFT){
                xPosition = this.textPaddingLeft;
                this.textPaint.setTextAlign(Paint.Align.LEFT);
            }
            else if(this.textHorizontalAlign == TextHorizontalAlign.RIGHT){
                xPosition = this.actualWidth - this.textPaddingRight;
                this.textPaint.setTextAlign(Paint.Align.RIGHT);
            }
            else{
                xPosition = this.actualWidth / 2;
                this.textPaint.setTextAlign(Paint.Align.CENTER);
            }

            if(this.textVerticalAlign == TextVerticalAlign.TOP){
                yPosition = this.textPaddingTop + this.textHeight;
            }
            else if(this.textVerticalAlign == TextVerticalAlign.BOTTOM){
                yPosition = this.actualHeight - this.textPaddingBottom;
            }
            else{
                yPosition = this.actualHeight - this.textSize;
            }
            canvas.drawText(this.text, xPosition, yPosition, this.textPaint);
        }
        if(this.centerParent){
            float pivotX = ((View)getParent()).getPivotX();
            float pivotY = ((View)getParent()).getPivotY();
            setX(pivotX - (float)(this.actualWidth / 2));
            setY(pivotY - (float)(this.actualHeight / 2));      // TODO
        }
    }
    protected void drawCircleButton(Canvas canvas){

    }
    @Override
    protected void onMeasure(int widthMeasureSpec, int heightMeasureSpec) {
        super.onMeasure(widthMeasureSpec, heightMeasureSpec);
        if(this.hasText){
            this.textPaint.getTextBounds(this.text, 0, text.length(), this.textBound);
            this.textWidth = this.textBound.width();
            this.textHeight = this.textBound.height();
            this.actualWidth = this.textWidth + this.textPaddingLeft + this.textPaddingRight;
            this.actualHeight = this.textHeight + this.textPaddingTop + this.textPaddingBottom; // TODO
        }
        else{
            this.actualWidth = Default.NO_TEXT_WIDTH;
            this.actualHeight = Default.NO_TEXT_HEIGHT;
        }
        int widthMode = MeasureSpec.getMode(widthMeasureSpec);
        int widthSize = MeasureSpec.getSize(widthMeasureSpec);
        int heightMode = MeasureSpec.getMode(heightMeasureSpec);
        int heightSize = MeasureSpec.getSize(heightMeasureSpec);

        if (widthMode == MeasureSpec.EXACTLY) {
            this.actualWidth = widthSize;
//            if(!this.centerParent){
//
//            }
        }
        else if (widthMode == MeasureSpec.AT_MOST) {
            // TODO
        }
        else {
            // TODO
        }

        if (heightMode == MeasureSpec.EXACTLY) {
            this.actualHeight = heightSize;
//            if(!this.centerParent){
//
//            }
        }
        else if (heightMode == MeasureSpec.AT_MOST) {
            // TODO
        }
        else {
            // TODO
        }
        this.actualWidth += this.increaseWidthAmount;
        this.actualHeight += this.increaseHeightAmount;
        setMeasuredDimension(this.actualWidth, this.actualHeight);
    }

    // TODO: beta
    public void increaseWidth(int amount){
        this.increaseWidthAmount += amount;
        requestLayout();
    }

    // TODO: beta
    public void increaseHeight(int amount){
        this.increaseHeightAmount += amount;
        requestLayout();
    }
}