

using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;

namespace CDM.Common
{
    public class AnimatedCircle : Border
    {
        private DispatcherTimer timer;


        public static readonly DependencyProperty IntervalProperty =
            DependencyProperty.Register("Interval", typeof(TimeSpan), typeof(AnimatedCircle),
                new PropertyMetadata(TimeSpan.FromSeconds(0.5), OnIntervalChanged));

        public static readonly DependencyProperty OpacitiesProperty =
            DependencyProperty.Register("Opacities", typeof(double[]), typeof(AnimatedCircle),
                new PropertyMetadata(new double[] { 1, 0.5, 0.2 }));

        public static readonly DependencyProperty CurrentIndexProperty =
                DependencyProperty.Register("CurrentIndex", typeof(int), typeof(AnimatedCircle),
                    new PropertyMetadata(0));

        public TimeSpan Interval
        {
            get { return (TimeSpan)GetValue(IntervalProperty); }
            set { SetValue(IntervalProperty, value); }
        }

        public double[] Opacities
        {
            get { return (double[])GetValue(OpacitiesProperty); }
            set { SetValue(OpacitiesProperty, value); }
        }

        public int CurrentIndex
        {
            get { return (int)GetValue(CurrentIndexProperty); }
            set { SetValue(CurrentIndexProperty, value); }
        }

        static AnimatedCircle()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AnimatedCircle),
                new FrameworkPropertyMetadata(typeof(AnimatedCircle)));
        }

        public AnimatedCircle()
        {
            InitializeTimer();
        }

        private static void OnIntervalChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is AnimatedCircle AnimatedCircle)
            {
                AnimatedCircle.timer.Interval = (TimeSpan)e.NewValue;
            }
        }

        private void InitializeTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = Interval;
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (Opacities.Length == 0) return;
            if (CurrentIndex > Opacities.Length - 1)
            {
                CurrentIndex = 0;
            }
            Opacity = Opacities[CurrentIndex];
            CurrentIndex = (CurrentIndex + 1) % Opacities.Length;
        }

    }
}
